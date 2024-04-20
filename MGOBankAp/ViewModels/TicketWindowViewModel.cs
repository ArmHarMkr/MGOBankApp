using MGOBankApp.Domain.Entity;
namespace MGOBankApp.ViewModels
{
    public class TicketWindowViewModel
    {
        public List<OrderTicketEntity> BillTicketWindow = new();
        public List<OrderTicketEntity> CreditTicketWindow = new();
        public List<OrderTicketEntity> TaxTicketWindow = new();
        public IEnumerable<OrderTicketEntity> AllOrders;
    }
}
