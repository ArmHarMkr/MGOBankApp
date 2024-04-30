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
    [Route("ATM/{controller}/{action}")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(AppDbContext db, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _db = db;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult LoginATM()
        {
            return View();
        }

        public async Task<IActionResult> CheckingLogin(CardEntity cardEntity)
        {
            if (cardEntity != null)
            {
                TempData["ErrorMessage"] = "No card found";
            }
            AppUser exampleUser = await _userManager.GetUserAsync(User);
            AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == exampleUser.Id);
            CardEntity currentCard = currentUser.CardEntity;
            

        }
    }
}