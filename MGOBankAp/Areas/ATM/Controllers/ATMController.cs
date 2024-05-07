using MGOBank.Service.Interfaces;
using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MGOBankApp.Areas.ATM.Controllers
{
    [Authorize]
    [Area("ATM")]
    [Route("ATM/[controller]")]
    public class ATMController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardTransactions _cardTransactions;

        public ATMController(AppDbContext db, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, ICardTransactions cardActions)
        {
            _db = db;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _cardTransactions = cardActions;
        }

/*        [HttpGet("LoginATM")]
        public IActionResult LoginATM()
        {
            return View();
        }

        [HttpPost("CheckingLogin")]
        public async Task<IActionResult> CheckingLogin(CardEntity cardEntity)
        {
            AppUser? exampleUser = await _userManager.GetUserAsync(User);
            AppUser? currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            CardEntity? currentCard = currentUser.CardEntity;
            if(currentCard == null)
            {
                TempData["ErrorMessage"] = "You don't have a card. Create it at first";
                return RedirectToAction("LoginATM");
            }
            
            if(currentCard.CardNum == cardEntity.CardNum && currentCard.PinCode == cardEntity.PinCode)
            {
                return RedirectToAction("MainATM");
            }
            else
            {
                TempData["ErrorMessage"] = "Wrong Card Num or Pin Code";
                return RedirectToAction("LoginATM");
            }
        }*/

        public async Task<IActionResult> MainATM()
        {
            AppUser exampleUser = await _userManager.GetUserAsync(User);
            AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            try
            {
                CardEntity currentCard = currentUser.CardEntity;
                currentCard.AccessibleMoney = currentUser.CardEntity.AccessibleMoney;

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View(currentUser.CardEntity);
        }

        [HttpGet("ChangePinCode")]
        public async Task<IActionResult> ChangePinCode(string? id)
        {
            CardEntity? currentCard = _unitOfWork.Card.Get(c => c.CardId == id);
            AppUser? exampleUser = await _userManager.GetUserAsync(User);
            AppUser? currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            if (currentUser?.CardEntity != currentCard)
            {
                return BadRequest("DO NOT CHANGE OTHERS CARD PIN CODE");
            }

            if (currentCard == null)
            {
                TempData["ErrorMessage"] = "No card found";
                return View("MainATM");
            }
            return View(currentCard);
        }

        [HttpPost("ChangePinCode")]
        public async Task<IActionResult> ChangePinCode(CardEntity cardEntity)
        {
            AppUser exampleUser = await _userManager.GetUserAsync(User);
            AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            CardEntity currentCard = currentUser.CardEntity;
            if (currentCard == null) { return BadRequest("No card found"); }
            await _cardTransactions.ChangePinCode(cardEntity, currentUser);
            return View("MainATM");
        }

        [HttpPost("WithdrawCash")]
        public async Task<IActionResult> WithdrawCash(CardEntity cardEntity)
        {
            AppUser exampleUser = await _userManager.GetUserAsync(User);
            AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            CardEntity currentCard = currentUser.CardEntity;
            if (currentCard == null) { return NotFound("No card found"); }

            try
            {
                if (currentUser != null && currentUser.CardEntity != null)
                {
                    if (currentCard.AccessibleMoney > cardEntity.AccessibleMoney)
                    {
                        await _cardTransactions.WithdrawCash(cardEntity, currentUser);
                        TempData["SuccessMessage"] = "Withdrawed successfully";
                        return RedirectToAction("MainATM");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Not enough accessible balance";
                        return RedirectToAction("MainATM");
                    }

                }
                return NotFound("No user or card found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }


        [HttpPost("CashIn")]
        public async Task<IActionResult> CashIn(CardEntity cardEntity)
        {
            try
            {
                AppUser exampleUser = await _userManager.GetUserAsync(User);
                AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
                if (currentUser != null && currentUser.CardEntity != null)
                {
                    CardEntity currentCard = currentUser.CardEntity;
                    currentCard.AccessibleMoney += cardEntity.AccessibleMoney;

                    _db.Cards.Update(currentCard);
                    TransactionEntity cashInAction = new TransactionEntity();
                    cashInAction.AppUser = currentUser;
                    cashInAction.TransactionType = Domain.Enum.TransactionType.CashIn;
                    cashInAction.ChangingMoney = cardEntity.AccessibleMoney;
                    await _unitOfWork.Transaction.Add(cashInAction);
                    await _unitOfWork.Save();

                    TempData["SuccessMessage"] = "Balance was added successfully";
                    return RedirectToAction("MainATM");
                }
                return NotFound("No user or card found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }
    }
}