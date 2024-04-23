using MGOBankApp.Domain.Entity;

namespace MGOBankApp.Areas.Admin.ViewModels
{
    public class AppuserCardViewModel
    {
        public AppUser AppUser { get; set; }
        public IEnumerable<AppUser> Users { get; set; }
        public CardEntity CardEntity { get; set; }
        public IEnumerable<CardEntity> Cards { get; set; }
    }
}
