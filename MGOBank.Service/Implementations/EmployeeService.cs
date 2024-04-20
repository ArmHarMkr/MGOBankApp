using MGOBank.Service.Interfaces;
using MGOBankApp.DAL.Data;
using MGOBankApp.Domain.Entity;
using MGOBankApp.Domain.Roles;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBank.Service.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        List<string> removingRoles = new();

        public EmployeeService(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task GiveBillEmployeeRole(AppUser appUser)
        {
            removingRoles.Add(SD.Role_CreditEmployee);
            removingRoles.Add(SD.Role_Customer);
            removingRoles.Add(SD.Role_TaxEmployee);
            await _userManager.RemoveFromRolesAsync(appUser, removingRoles);
            await _userManager.AddToRoleAsync(appUser, SD.Role_BillEmployee);
        }

        public async Task GiveCreditEmployeeRole(AppUser appUser)
        {
            removingRoles.Add(SD.Role_BillEmployee);
            removingRoles.Add(SD.Role_Customer);
            removingRoles.Add(SD.Role_TaxEmployee);
            await _userManager.RemoveFromRolesAsync(appUser, removingRoles);
            await _userManager.AddToRoleAsync(appUser, SD.Role_CreditEmployee);
        }

        public async Task GiveTaxEmployeeRole(AppUser appUser)
        {
            removingRoles.Add(SD.Role_BillEmployee);
            removingRoles.Add(SD.Role_Customer);
            removingRoles.Add(SD.Role_CreditEmployee);
            await _userManager.RemoveFromRolesAsync(appUser, removingRoles);
            await _userManager.AddToRoleAsync(appUser, SD.Role_TaxEmployee);
        }

        public async Task GiveCustomerRole(AppUser appUser)
        {
            removingRoles.Add(SD.Role_BillEmployee);
            removingRoles.Add(SD.Role_CreditEmployee);
            removingRoles.Add(SD.Role_TaxEmployee);
            await _userManager.RemoveFromRolesAsync(appUser, removingRoles);
            await _userManager.AddToRoleAsync(appUser, SD.Role_Customer);
        }
    }
}
