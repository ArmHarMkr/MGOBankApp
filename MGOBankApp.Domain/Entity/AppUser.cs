<<<<<<< HEAD
﻿using Microsoft.AspNet.Identity.EntityFramework;
=======
﻿using Microsoft.AspNetCore.Identity;
>>>>>>> 392dccbe256d96c3f3f6442c97bef3c51f16a71c
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MGOBankApp.Domain.Entity
{
    public class AppUser : IdentityUser
    {
<<<<<<< HEAD
=======
        public string FullName { get;set; }
        public long AccountNumber { get;set; }
        [AllowNull]
        public CardEntity CardEntity { get;set; }
>>>>>>> 392dccbe256d96c3f3f6442c97bef3c51f16a71c
    }
}
