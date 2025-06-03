using EstateFin.Data;
using EstateFin.ILeaseRepo;
using EstateFin.Models;
using EstateFin.Repositories;
using EstateFin.Services;
using EstatePro.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
        options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
    });

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<MailSettings>>().Value);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<PropertyRepo, PropertyService>();

builder.Services.AddScoped<ILeaseRepo, LeaseServices>();
builder.Services.AddScoped<ILeaseTenantRepo,LeaseTenantService>();



builder.Services.AddScoped<IBookingRepository, BookingService>();
builder.Services.AddScoped<ITransactionRepository, TransactionService>();

builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseRouting();

// ✅ Order matters: Authentication BEFORE Session
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// ✅ Optional: Fix SameSite issue with Chrome
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
