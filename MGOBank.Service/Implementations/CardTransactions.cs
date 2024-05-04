using MGOBank.Service.Interfaces;
using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using MGOBankApp.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBank.Service.Implementations
{
    public class CardTransactions : ICardTransactions
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public CardTransactions(
            AppDbContext db,
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork
                               )
        {
            _db = db;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }


        public async Task ChangePinCode(CardEntity cardEntity, AppUser currentUser)
        {
            CardEntity currentCard = currentUser.CardEntity;
            if (currentCard == null)
            {
                throw new Exception("No card found");
            }

            currentCard.PinCode = cardEntity.PinCode;
            _db.Cards.Update(currentCard);
            TransactionEntity changePinAction = new TransactionEntity();
            changePinAction.AppUser = currentUser;
            changePinAction.TransactionType = TransactionType.Withdraw;
            await _unitOfWork.Transaction.Add(changePinAction);
            await _unitOfWork.Save();
        }

        public double ShowBalance(CardEntity card)
        {
            return card.AccessibleMoney;
        }

        public async Task WithdrawCash(CardEntity cardEntity, AppUser currentUser)
        {
            CardEntity currentCard = currentUser.CardEntity;
            currentCard.AccessibleMoney -= cardEntity.AccessibleMoney;
            _db.Cards.Update(currentCard);
            TransactionEntity withdrawAction = new();
            withdrawAction.AppUser = currentUser;
            withdrawAction.TransactionType = TransactionType.Withdraw;
            withdrawAction.ChangingMoney = cardEntity.AccessibleMoney;
            await _unitOfWork.Save();            
        }
    }
}
