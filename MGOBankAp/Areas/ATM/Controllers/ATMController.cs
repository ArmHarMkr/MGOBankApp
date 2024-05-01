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
    [Route("ATM/[controller]/[action]")]
    public class ATMController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ATMController(AppDbContext db, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _db = db;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("LoginATM")]
        public IActionResult LoginATM()
        {
            return View();
        }

        [HttpPost("CheckingLogin")]
        public async Task<IActionResult> CheckingLogin(CardEntity cardEntity)
        {
            AppUser exampleUser = await _userManager.GetUserAsync(User);
            AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            CardEntity currentCard = currentUser.CardEntity;
            if(currentCard == null)
            {
                TempData["SuccessMessage"] = "You don't have a card. Create it at first";
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
        }


        [HttpGet("MainATM")]
        public async Task<IActionResult> MainATM()
        {
            AppUser exampleUser = await _userManager.GetUserAsync(User);
            AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            CardEntity currentCard = currentUser.CardEntity;
            return View(currentCard);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(CardEntity cardEntity)
        {
            AppUser exampleUser = await _userManager.GetUserAsync(User);
            AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            CardEntity currentCard = currentUser.CardEntity;
            if (currentCard == null) { return BadRequest("No card found"); }

            currentCard.PinCode = cardEntity.PinCode;
            return View("MainATM");
        }

        [HttpPost("WithdrawCash")]
        public async Task<IActionResult> WithdrawCash(CardEntity cardEntity)
        {
            AppUser exampleUser = await _userManager.GetUserAsync(User);
            AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            CardEntity currentCard = currentUser.CardEntity;
            if (currentCard == null) { return BadRequest("No card found"); }

            try
            {
                if (currentUser != null && currentUser.CardEntity != null)
                {
                    if (currentCard.AccessibleMoney > cardEntity.AccessibleMoney)
                    {
                        currentCard.AccessibleMoney -= cardEntity.AccessibleMoney;
                        _db.Cards.Update(currentCard);
                        await _unitOfWork.Save();

                        TempData["SuccessMessage"] = "Withdrawed successfully";
                        return RedirectToAction("MainATM");
                    }
                    else
                    {
                        TempData["ErroMessage"] = "Not enough accessible balance";
                        return RedirectToAction("MainATM");
                    }

                }
                return BadRequest("No user or card found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        public async Task<IActionResult> CashIn(CardEntity cardEntity)
        {
            AppUser exampleUser = await _userManager.GetUserAsync(User);
            AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            CardEntity currentCard = currentUser.CardEntity;
            if (currentCard == null) { return BadRequest("No card found"); }

            try
            {
                if (currentUser != null && currentUser.CardEntity != null)
                {
                    currentCard.AccessibleMoney -= cardEntity.AccessibleMoney;
                    _db.Cards.Update(currentCard);
                    await _unitOfWork.Save();

                    TempData["SuccessMessage"] = "Proccessed successfully";
                    return RedirectToAction("MainATM");
                }
                return BadRequest("No user or card found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        public async Task<IActionResult> ShowBalance(string? id)
        {
            CardEntity? currentCard = _unitOfWork.Card.Get(c => c.CardId == id);
            AppUser? exampleUser = await _userManager.GetUserAsync(User);
            AppUser? currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            if(currentUser?.CardEntity != currentCard)
            {
                return BadRequest("DO NOT WATCH OTHERS CARD BALANCE");
            }

            if (currentCard == null)
            {
                TempData["ErrorMessage"] = "No card found";
                return View("MainATM");
            }
            return View(currentCard);
        }
    }
}