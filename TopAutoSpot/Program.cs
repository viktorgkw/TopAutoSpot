using Hangfire;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Services.EmailService;
using TopAutoSpot.Services.NewsServices;
using TopAutoSpot.Services.AuctionServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

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
});

// Razor Pages Configuration
builder.Services.AddRazorPages(options =>
{
    options.RootDirectory = "/Views";
});

// Services DI Configuration
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<INewsService, NewsService>();
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

var app = builder.Build();

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

// Daily Background Auction Reminder
RecurringJob.AddOrUpdate<IAuctionService>(s => s.DailyCheckAndRemind(), Cron.Daily);

app.Run();