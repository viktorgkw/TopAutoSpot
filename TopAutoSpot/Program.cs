using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Services.EmailService;
using TopAutoSpot.Services.NewsServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
});

builder.Services.AddRazorPages(options =>
{
    options.RootDirectory = "/Views";
});

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<INewsService, NewsService>();

builder.Services.AddControllersWithViews();

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

app.Run();