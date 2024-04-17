using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBankApp.DAL.Repository
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        Task Save();
    }
}
