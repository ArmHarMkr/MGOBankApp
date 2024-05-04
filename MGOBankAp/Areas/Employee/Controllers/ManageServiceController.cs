using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using MGOBankApp.Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MGOBankApp.Areas.Employee.Controllers
{
    [Authorize]
    [Route("Employee/{controller}")]
    [Area("Employee")]
    public class ManageServiceController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ManageServiceController(AppDbContext db, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _db = db;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }


        [Authorize(Roles = "CreditEmployee")]
        [HttpGet("CreditServices")]
        public async Task<IActionResult> CreditServices()
        {
            IEnumerable<OrderTicketEntity> allCredits = _db.OrderTickets.Include(o => o.AppUser)
                                                                        .Where(o => o.OrderService == Domain.Enum.Services.Credit)
                                                                        .Where(o => o.IsDone == false)
                                                                        .OrderBy(o => o.OrderNumber);
            return View(allCredits);
        }

        [Authorize(Roles = "TaxEmployee")]
        [HttpGet("TaxServices")]
        public async Task<IActionResult> TaxServices()
        {
            IEnumerable<OrderTicketEntity> allTaxes = _db.OrderTickets.Include(o => o.AppUser)
                                                                      .Where(o => o.OrderService == Domain.Enum.Services.Tax)
                                                                      .Where(o => o.IsDone == false)
                                                                      .OrderBy(o => o.OrderNumber);
            return View(allTaxes);
        }

        [Authorize(Roles = "BillEmployee")]
        [HttpGet("BillServices")]
        public async Task<IActionResult> BillServices()
        {
            IEnumerable<OrderTicketEntity> allBills = _db.OrderTickets.Include(o => o.AppUser)
                                                                        .Where(o => o.OrderService == Domain.Enum.Services.Bill)
                                                                        .Where(o => o.IsDone == false)
                                                                        .OrderBy(o => o.OrderNumber);
            return View(allBills);
        }


        [HttpPost("CheckOrder")]
        public async Task<IActionResult> CheckOrder()
        {
            OrderTicketEntity lastOrder = await _db.OrderTickets.Where(o => o.IsDone == false).OrderByDescending(c => c.OrderNumber).LastAsync();
            if (lastOrder == null) { return BadRequest("No ticket found"); }

            lastOrder.IsDone = true;
            _db.OrderTickets.Update(lastOrder);
            await _unitOfWork.Save();
            
            if(lastOrder.OrderService == Domain.Enum.Services.Credit){ return RedirectToAction("CreditServices"); }
            else if(lastOrder.OrderService == Domain.Enum.Services.Tax) { return RedirectToAction("TaxServices"); }
            else { return RedirectToAction("BillServices"); }
        }
    }
}
