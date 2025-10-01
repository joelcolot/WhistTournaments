using Microsoft.AspNetCore.Authentication.Cookies;
using WhistTournaments.BLL.Services;
using WhistTournaments.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath="/User/Login";
        options.LogoutPath="/Home/Index";
        options.AccessDeniedPath="/Home/Index";
        options.ExpireTimeSpan=TimeSpan.FromHours(24);
    });

builder.Services.AddSession();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TournamentRepository>();
builder.Services.AddScoped<GameRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TournamentService>();
builder.Services.AddScoped<GameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tournament}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
