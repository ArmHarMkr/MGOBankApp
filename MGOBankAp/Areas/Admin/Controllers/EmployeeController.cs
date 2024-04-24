using MGOBank.Service.Interfaces;
using MGOBankApp.Areas.Admin.ViewModels;
using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using MGOBankApp.Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MGOBankApp.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    [Route("admin/{controller}")]
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<AppUser> _userManager;

        public EmployeeController(AppDbContext db,
                                  UserManager<AppUser> userManager,
                                  IUnitOfWork unitOfWork,
                                  IEmployeeService employeeService)
        {
            _db = db;
            _employeeService = employeeService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> AllUsers()
        {
            IEnumerable<AppUser> allUsers = await _userManager.Users.ToListAsync();
            AppuserCardViewModel appuserCardViewModel = new();
            appuserCardViewModel.Users = allUsers;
            return View(appuserCardViewModel);
        }

        [HttpPost("GiveTaxEmployee")]
        public async Task<IActionResult> GiveTaxEmployee(string? id)
        {
            var userFromDb = _unitOfWork.Employee.Get(e => e.Id == id);
            if (userFromDb == null)
                return NotFound();
            await _employeeService.GiveTaxEmployeeRole(userFromDb);
            return RedirectToAction("AllUsers");
        }

        [HttpPost("GiveBillEmployee")]
        public async Task<IActionResult> GiveBillEmployee(string? id)
        {
            var userFromDb = _unitOfWork.Employee.Get(e => e.Id == id);
            if (userFromDb == null)
                return NotFound();
            await _employeeService.GiveBillEmployeeRole(userFromDb);
            return RedirectToAction("AllUsers");
        }

        [HttpPost("GiveCreditEmployee")]
        public async Task<IActionResult> GiveCreditEmployee(string? id)
        {
            var userFromDb = _unitOfWork.Employee.Get(e => e.Id == id);
            if (userFromDb != null)
            {
                await _employeeService.GiveCreditEmployeeRole(userFromDb);
                return RedirectToAction("AllUsers");
            }
            return NotFound();
        }


        [HttpPost("GiveCustomerRole")]
        public async Task<IActionResult> GiveCustomerRole(string? id)
        {
            var userFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (userFromDb == null)
                return NotFound();
            await _employeeService.GiveCustomerRole(userFromDb);
            return RedirectToAction("AllUsers");
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string? id)
        {
            try
            {
                AppUser deletingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                AppUser currentUser = await _userManager.GetUserAsync(User);
                if(await _userManager.IsInRoleAsync(currentUser, SD.Role_Admin))
                {
                    if (deletingUser != null)
                    {
                        if (deletingUser.CardEntity != null)
                        {
                            CardEntity userCard = await _db.Cards.FirstOrDefaultAsync(c => c.CardId == deletingUser.CardEntity.CardId);
                            if (userCard != null)
                            {
                                _db.Cards.Remove(userCard);
                            }
                        }

                        _db.Users.Remove(deletingUser);
                        await _db.SaveChangesAsync();
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Do not delete yourself, bro";
                }
                return RedirectToAction("AllUsers");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToBalance(string? id, CardEntity cardEntity)
        {
            try
            {
                AppUser currentUser = await _db.Users.Include(u => u.CardEntity).FirstOrDefaultAsync(u => u.Id == id);
                if (currentUser != null && currentUser.CardEntity != null)
                {
                    CardEntity currentCard = currentUser.CardEntity;

                    currentCard.AccessibleMoney += cardEntity.AccessibleMoney;

                    _db.Cards.Update(currentCard);
                    await _unitOfWork.Save();

                    TempData["SuccessMessage"] = "Balance was added successfully";
                    return RedirectToAction("AllUsers");
                }
                return BadRequest("No user or card found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }


    }
}
