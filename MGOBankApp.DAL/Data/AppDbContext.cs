<<<<<<< HEAD
﻿using System;
=======
﻿using MGOBankApp.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
>>>>>>> 392dccbe256d96c3f3f6442c97bef3c51f16a71c
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBankApp.DAL.Data
{
<<<<<<< HEAD
    public class AppDbContext
    {
=======
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CardEntity> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
>>>>>>> 392dccbe256d96c3f3f6442c97bef3c51f16a71c
    }
}
