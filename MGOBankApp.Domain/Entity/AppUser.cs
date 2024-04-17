using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace MGOBankApp.Domain.Entity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get;set; }
        public long AccountNumber { get;set; }
        [AllowNull]
        public CardEntity CardEntity { get;set; }
    }
}
