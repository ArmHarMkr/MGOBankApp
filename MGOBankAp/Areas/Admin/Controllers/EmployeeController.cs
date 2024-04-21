using MGOBank.Service.Interfaces;
using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using MGOBankApp.Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public EmployeeController(AppDbContext db, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IEmployeeService employeeService)
        {
            _db = db;
            _employeeService = employeeService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> AllUsers()
        {
            IEnumerable<AppUser> allUsers = await _userManager.Users.ToListAsync();
            return View(allUsers);
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


    }
}
