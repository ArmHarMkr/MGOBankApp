using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
<<<<<<< HEAD
=======
using MGOBankApp.Domain.Entity;
using Microsoft.AspNetCore.Identity;
>>>>>>> 392dccbe256d96c3f3f6442c97bef3c51f16a71c
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


        public UnitOfWork(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            User = new UserRepository(_db);
        }


        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
