using MGOBankApp.Domain.Entity;

namespace MGOBankApp.ViewModels
{
    public class TransactionAppUserViewModel
    {
        public AppUser AppUser { get; set; }
        public TransactionEntity TransactionEntity{ get; set; }
        public IEnumerable<TransactionEntity> CashInActions { get; set; }
        public IEnumerable<TransactionEntity> WithdrawActions { get; set; }
        public IEnumerable<TransactionEntity> PinCodeChangeActions { get; set; }
        public IEnumerable<TransactionEntity> AllTransactions { get; set; }
    }
}
