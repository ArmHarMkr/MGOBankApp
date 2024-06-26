﻿using MGOBankApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBankApp.DAL.Repository
{
    public interface IOrderTicketRepository : IRepository<OrderTicketEntity>
    {
       Task BillService(AppUser user);
       Task TaxService(AppUser user);
       Task CreditService(AppUser user);
    }
}
