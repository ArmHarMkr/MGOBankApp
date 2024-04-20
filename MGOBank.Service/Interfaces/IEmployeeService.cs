using MGOBankApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBank.Service.Interfaces
{
    public interface IEmployeeService
    {
        Task GiveTaxEmployeeRole(AppUser appUser);
        Task GiveBillEmployeeRole(AppUser appUser);
        Task GiveCreditEmployeeRole(AppUser appUser);
        Task GiveCustomerRole(AppUser appUser);
    }
}
