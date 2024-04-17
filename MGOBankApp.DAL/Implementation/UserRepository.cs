using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBankApp.DAL.Implementation
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        private AppDbContext _db;
        private Random rnd = new();
        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public long AddUserAccountNumber()
        {
            long accountNumber = 0;
            if (_db.Users.Select(u => u.AccountNumber).Any())
            {
                accountNumber = _db.Users.Select(u => u.AccountNumber).Max() + 1;
            }
            else
            {
                accountNumber = 209800000000000; // Initial account number if no users exist
            }

            return accountNumber;
        }
    }
}