using MGOBankApp.DAL.Implementation;
using MGOBankApp.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using MGOBankApp.Domain.Entity;
using MGOBankApp.DAL.Data;
using MGOBank.Service.Interfaces;
using MGOBank.Service.Implementations;
using Microsoft.AspNetCore.Identity;
using MGOBankApp.Domain.Roles;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOrderTicketRepository, OrderTicketRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ILunaCounter, LunaCounter>();
builder.Services.AddTransient<ICardNumberGenerator, CardNumberGenerator>();

//Adding background Service
builder.Services.AddHostedService<OrderTicketUpdateService>();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

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

app.MapAreaControllerRoute(
    name: "admin_area",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
);



var logger = app.Services.GetService<ILogger<Program>>();
logger?.LogInformation("Starting program...");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] {SD.Role_Customer, SD.Role_Admin, SD.Role_TaxEmployee, SD.Role_BillEmployee, SD.Role_CreditEmployee };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    string email = "admin@admin.com";
    if (await userManager.FindByEmailAsync(email) != null)
    {
        var user = await userManager.FindByEmailAsync(email);
        await userManager.AddToRoleAsync(user, SD.Role_Admin);
        await userManager.RemoveFromRoleAsync(user, SD.Role_Customer);
    }
}

app.Run();
