using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Services.AuctionServices;
using TopAutoSpot.Services.EmailService;
using TopAutoSpot.Services.NewsServices;
using TopAutoSpot.Services.PaymentServices;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity Configuration
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// Identity Options Configuration
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = true;
});

// Razor Pages Configuration
builder.Services.AddRazorPages(options =>
{
    options.RootDirectory = "/Views";
});

// Services DI Configuration
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddTransient<IAuctionService, AuctionService>();

builder.Services.AddControllersWithViews();

// Hangfire Configuration
builder.Services.AddHangfire(config =>
{
    config.UseSimpleAssemblyNameTypeSerializer();
    config.UseRecommendedSerializerSettings();
    config.UseSqlServerStorage(connectionString);
});
builder.Services.AddHangfireServer();

// Stripe Configuration
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Bad/Invalid URLs will redirect the user to /NotFound page.
app.UseStatusCodePages();
app.UseStatusCodePagesWithRedirects("/NotFound");

app.UseAuthorization();

app.MapRazorPages();

app.UseHangfireDashboard();
app.MapHangfireDashboard();

// Background Auctions Tasks
RecurringJob.AddOrUpdate<IAuctionService>(s => s.DailyCheckAndRemind(), Cron.Daily);
RecurringJob.AddOrUpdate<IAuctionService>(s => s.StartingAuctionsCheck(), Cron.Minutely);

app.Run();