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
    [Authorize(Roles = "TaxEmployee, BillEmployee, CreditEmployee")]
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
                                                                        .Where(o => o.IsDone == false);
            return View(allCredits);
        }

        [Authorize(Roles = "TaxEmployee")]
        [HttpGet("TaxServices")]
        public async Task<IActionResult> TaxServices()
        {
            IEnumerable<OrderTicketEntity> allTaxes = _db.OrderTickets.Include(o => o.AppUser)
                                                                      .Where(o => o.OrderService == Domain.Enum.Services.Tax)
                                                                      .Where(o => o.IsDone == false);
            return View(allTaxes);
        }

        [Authorize(Roles = "BillService")]
        [HttpGet("BillServices")]
        public async Task<IActionResult> BillServices()
        {
            IEnumerable<OrderTicketEntity> allBills = _db.OrderTickets.Include(o => o.AppUser)
                                                                        .Where(o => o.OrderService == Domain.Enum.Services.Credit)
                                                                        .Where(o => o.IsDone == false);
            return View(allBills);
        }


        [HttpPost("CheckOrder")]
        public async Task<IActionResult> CheckOrder(string? id)
        {
            OrderTicketEntity order = _unitOfWork.OrderTicket.Get(o => o.OrderTicketId == id);
            if (order == null) { return BadRequest("No ticket found"); }

            order.IsDone = true;
            _db.OrderTickets.Update(order);
            await _unitOfWork.Save();
            
            if(order.OrderService == Domain.Enum.Services.Credit){ return RedirectToAction("CreditServices"); }
            else if(order.OrderService == Domain.Enum.Services.Tax) { return RedirectToAction("TaxServices"); }
            else { return RedirectToAction("BillServices"); }
        }
    }
}
