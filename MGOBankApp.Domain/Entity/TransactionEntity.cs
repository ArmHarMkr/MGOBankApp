using MGOBankApp.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBankApp.Domain.Entity
{
    public class TransactionEntity
    {
        [Key]
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();
        public AppUser AppUser { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime ActionTime { get; set; } = DateTime.Now;
    }
}
