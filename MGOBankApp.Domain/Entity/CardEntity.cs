using MGOBankApp.Domain.Enum;
<<<<<<< HEAD
=======
using System.ComponentModel.DataAnnotations;
>>>>>>> 392dccbe256d96c3f3f6442c97bef3c51f16a71c
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
<<<<<<< HEAD
        public double AccessibleMoney { get; set; } = 0;
        [AllowNull]
=======
        [AllowNull]
        public double AccessibleMoney { get; set; } = 0;
        [AllowNull]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "The field must be exactly 4 characters long.")]
>>>>>>> 392dccbe256d96c3f3f6442c97bef3c51f16a71c
        public string PinCode { get; set; }
        public DateTime CardCreatedTime { get; set; } = DateTime.Now;
    }
}
