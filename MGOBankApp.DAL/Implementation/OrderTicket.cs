using MGOBankApp.DAL.Data;
using MGOBankApp.DAL.Repository;
using MGOBankApp.Domain.Entity;
using MGOBankApp.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBankApp.DAL.Implementation;

public class OrderTicket : Repository<OrderTicketEntity>, IOrderTicket
{
    private readonly AppDbContext _db;
    public OrderTicket(AppDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task BillService(AppUser user)
    {
        int nextOrderNumber = 1;

        int maxOrderNumber = await _db.OrderTickets
                                      .Where(o => o.OrderService == Services.Bill)
                                      .MaxAsync(o => (int?)o.OrderNumber) ?? 0;

        nextOrderNumber = maxOrderNumber + 1;

        // Create a new OrderTicketEntity with the next order number
        OrderTicketEntity newOrder = new OrderTicketEntity
        {
            OrderService = Services.Bill,
            OrderNumber = nextOrderNumber,
            AppUser = user
        };

        // Add the new order to the database
        _db.OrderTickets.Add(newOrder);
        await _db.SaveChangesAsync();
    }

    public async Task CreditService(AppUser user)
    {
        int nextOrderNumber = 1;

        int maxOrderNumber = await _db.OrderTickets
                                      .Where(o => o.OrderService == Services.Credit)
                                      .MaxAsync(o => (int?)o.OrderNumber) ?? 0;

        nextOrderNumber = maxOrderNumber + 1;

        // Create a new OrderTicketEntity with the next order number
        OrderTicketEntity newOrder = new OrderTicketEntity
        {
            OrderService = Services.Credit,
            OrderNumber = nextOrderNumber,
            AppUser = user
        };

        // Add the new order to the database
        _db.OrderTickets.Add(newOrder);
        await _db.SaveChangesAsync();
    }

    public async Task TaxService(AppUser user)
    {
        int nextOrderNumber = 1;

        int maxOrderNumber = await _db.OrderTickets
                                      .Where(o => o.OrderService == Services.Tax)
                                      .MaxAsync(o => (int?)o.OrderNumber) ?? 0;

        nextOrderNumber = maxOrderNumber + 1;

        // Create a new OrderTicketEntity with the next order number
        OrderTicketEntity newOrder = new OrderTicketEntity
        {
            OrderService = Services.Tax,
            OrderNumber = nextOrderNumber,
            AppUser = user
        };

        _db.OrderTickets.Add(newOrder);
        await _db.SaveChangesAsync();
    }
}