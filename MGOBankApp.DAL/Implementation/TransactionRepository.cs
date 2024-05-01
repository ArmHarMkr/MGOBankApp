using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBankApp.DAL.Implementation
{
    public class TransactionRepository : Repository<TransactionEntity>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext db) : base(db)
        {
        }
    }
}
