using MGOBankApp.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBankApp.Domain.Entity
{
    public class OrderTicketEntity
    {
        [Key]
        public string OrderTicketId { get; set; } = Guid.NewGuid().ToString();
        public Services OrderService { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int OrderNumber { get; set; }
        public bool IsDone { get; set; } = false;
        public AppUser AppUser { get; set; }
    }
}
