﻿using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MGOBankApp.Controllers
{
    public class ServiceOrderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderTicket _orderTicket;

        public ServiceOrderController(AppDbContext db, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IOrderTicket orderTicket)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _orderTicket = orderTicket;
        }

        public IActionResult AllServices()
        {
            return View();
        }

        public async Task<IActionResult> AllOrders()
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);
            IEnumerable<OrderTicketEntity> orders = _db.OrderTickets
                                                       .Include(o => o.AppUser)
                                                       .Where(o => o.IsDone == false)
                                                       .Where(o => o.AppUser == currentUser);
            return View(orders);
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
    }
}
