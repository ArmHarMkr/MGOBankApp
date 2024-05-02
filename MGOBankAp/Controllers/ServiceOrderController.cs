using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using MGOBankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MGOBankApp.Controllers
{
    [Authorize]
    public class ServiceOrderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderTicketRepository _orderTicket;

        public ServiceOrderController(AppDbContext db, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IOrderTicketRepository orderTicket)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _orderTicket = orderTicket;
        }

        public async Task<IActionResult> AllOrders()
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);
            IEnumerable<OrderTicketEntity> orders = _db.OrderTickets
                                                       .Include(o => o.AppUser)
                                                       .Where(o => o.IsDone == false)
                                                       .OrderBy(o => o.OrderDate);
            TicketWindowViewModel ticketWindowVM = new();
            ticketWindowVM.AllOrders = orders.Where(o => o.AppUser == currentUser);
            ticketWindowVM.AllUsersOrders = orders.ToList();
            foreach (var order in orders)
            {
                if (order.OrderService.ToString().ToLower() == "bill")
                    ticketWindowVM.BillTicketWindow.Add(order);
                else if(order.OrderService.ToString().ToLower() == "credit")
                    ticketWindowVM.CreditTicketWindow.Add(order);
                else
                    ticketWindowVM.TaxTicketWindow.Add(order);

            }   
            return View(ticketWindowVM);
        }

        [HttpPost("AddBillService")]
        public async Task<IActionResult> AddBillService()
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);
            try
            {
                await _orderTicket.BillService(currentUser);
                return RedirectToAction("AllOrders", "ServiceOrder");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddCreditService")]
        public async Task<IActionResult> AddCreditService()
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);
            try
            {
                await _orderTicket.CreditService(currentUser);
                return RedirectToAction("AllOrders", "ServiceOrder");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddTaxService")]
        public async Task<IActionResult> AddTaxService()
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);
            try
            {
                await _orderTicket.TaxService(currentUser);
                return RedirectToAction("AllOrders", "ServiceOrder");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeleteTicket")]
        public async Task<IActionResult> DeleteTicket(string? id)
        {
            OrderTicketEntity orderFromDb = _unitOfWork.OrderTicket.Get(o => o.OrderTicketId == id);
            if (orderFromDb != null)
            {
                orderFromDb.IsDone = true;
                _db.OrderTickets.Update(orderFromDb);
                await _unitOfWork.Save();
            }
            return RedirectToAction("AllOrders", "ServiceOrder");
        }

        [HttpPost("DeleteAll")]
        public async Task<IActionResult> DeleteAll()
        {
            _unitOfWork.OrderTicket.RemoveRange(_unitOfWork.OrderTicket.GetAll());
            await _unitOfWork.Save();
            return RedirectToAction("AllOrders", "ServiceOrder");
        }
    }
}
