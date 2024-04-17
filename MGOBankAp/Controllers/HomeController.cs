using MGOBank.Service.Interfaces;
using MGOBankAp.Models;
using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using MGOBankApp.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace MGOBankAp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardNumberGenerator _cardNumberGenerator;
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            IUnitOfWork unitOfWork,
            AppDbContext db,
            UserManager<AppUser> userManager,
            ICardNumberGenerator cardNumberGenerator
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _db = db;
            _userManager = userManager;
            _cardNumberGenerator = cardNumberGenerator;
        }

        public async Task<IActionResult> Index()
        {
            AppUser myUser = await _userManager.GetUserAsync(User);
            AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u == myUser);
            if (currentUser.CardEntity == null)
            {
                ViewBag.PaymentSystems = Enum.GetNames(typeof(PaymentSystem)).ToList();
                return View(currentUser);
            }
            else
            {
                return View(currentUser);
            }
        }

        [HttpPost("CreateCard")]
        public async Task<IActionResult> CreateCard(string paymentSystem)
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && currentUser.CardEntity == null)
            {

                // Create a new card for the user
                currentUser.CardEntity = new CardEntity
                {
                    CardNum = _cardNumberGenerator.GenerateCardNumber(paymentSystem),
                    PinCode = "1234",
                    AccessibleMoney = 0,
                    PaymentSystem = (PaymentSystem)Enum.Parse(typeof(PaymentSystem), paymentSystem)
                };
                await _userManager.UpdateAsync(currentUser);
            }
            return RedirectToAction("Index");
        }

    }
}
