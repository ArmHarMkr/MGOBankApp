using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBankApp.DAL.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;
        private UserManager<AppUser> _userManager;
        public IUserRepository User { get; private set; }
        public IOrderTicketRepository OrderTicket { get; private set; }
        public IEmployeeRepository Employee { get; private set; }


        public UnitOfWork(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            User = new UserRepository(_db);
            OrderTicket = new OrderTicketRepository(_db);
            Employee = new EmployeeRepository(_db, _userManager);
        }


        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
