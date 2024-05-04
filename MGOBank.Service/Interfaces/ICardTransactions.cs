using MGOBankApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBank.Service.Interfaces
{
    public interface ICardTransactions
    {
        public Task ChangePinCode(CardEntity card, AppUser currentUser);
        public Task WithdrawCash(CardEntity card, AppUser currentUser);
        public double ShowBalance(CardEntity card);
    }
}
