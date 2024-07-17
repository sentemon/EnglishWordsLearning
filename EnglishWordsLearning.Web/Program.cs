using EnglishWordsLearning.Web.ActionFilters;
using Microsoft.AspNetCore.Authentication.Cookies;
using EnglishWordsLearning.Infrastructure.Data;
using EnglishWordsLearning.Core.Interfaces;
using EnglishWordsLearning.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var conncectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(options =>
     options.UseNpgsql(conncectionString,
             dbContextOptionsBuilder => dbContextOptionsBuilder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Account/SignIn";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAccountService, AccountRepository>();
builder.Services.AddScoped<IHistoryLogs, HistoryLogsRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

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
    defaults: new { controller = "Profile", action = "Index", username="" });


app.Run();