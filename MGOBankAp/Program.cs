<<<<<<< HEAD
using MGOBankApp.DAL;
using MGOBankApp.DAL.Implementation;
using MGOBankApp.DAL.Repository;
using Microsoft.EntityFrameworkCore;

=======
using Microsoft.EntityFrameworkCore;
using MGOBankApp.Domain.Entity;
using MGOBankApp.DAL.Data;
using Microsoft.AspNetCore.Identity;
using MGOBankApp.DAL.Repository;
using MGOBankApp.DAL.Implementation;
using MGOBank.Service.Interfaces;
using MGOBank.Service.Implementations;
>>>>>>> 392dccbe256d96c3f3f6442c97bef3c51f16a71c
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ILunaCounter, LunaCounter>();
builder.Services.AddTransient<ICardNumberGenerator, CardNumberGenerator>();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapRazorPages();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
