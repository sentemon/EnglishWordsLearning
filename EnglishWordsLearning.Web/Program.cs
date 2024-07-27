using EnglishWordsLearning.Application.Services;
using EnglishWordsLearning.Web.ActionFilters;
using Microsoft.AspNetCore.Authentication.Cookies;
using EnglishWordsLearning.Infrastructure.Data;
using EnglishWordsLearning.Core.Interfaces;
using EnglishWordsLearning.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString,
        dbContextOptionsBuilder => dbContextOptionsBuilder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/SignIn";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// Register repositories and services
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IHistoryLogsRepository, HistoryLogsRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IHistoryLogsService, HistoryLogsService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddTransient<IMyMemoryService, MyMemoryService>();

builder.Services.AddScoped<AddUsernameToViewBagFilter>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AddUsernameToViewBagFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// Enable session middleware
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Profile",
    pattern: "Profile/{username}",
    defaults: new { controller = "Profile", action = "Index", username = "" });

app.Run();