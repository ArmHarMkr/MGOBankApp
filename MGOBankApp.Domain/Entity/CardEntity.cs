using MGOBankApp.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MGOBankApp.Domain.Entity
{
    public class CardEntity
    {
        [Key]
        public string CardId { get; set; } = Guid.NewGuid().ToString();
        public string CardNum { get; set; }
        [AllowNull]
        public PaymentSystem PaymentSystem { get; set; }
        [AllowNull]
        public double AccessibleMoney { get; set; } = 0;
        [AllowNull]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "The field must be exactly 4 characters long.")]
        public string PinCode { get; set; }
        public DateTime CardCreatedTime { get; set; } = DateTime.Now;
    }
}
