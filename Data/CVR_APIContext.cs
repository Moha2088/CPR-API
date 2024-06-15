using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CVR_API.Models;

namespace CVR_API.Data
{
    public class CVR_APIContext : DbContext
    {
        public CVR_APIContext (DbContextOptions<CVR_APIContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.LoginUser)
                .WithOne(u => u.User)
                .HasForeignKey<LoginUser>(u => u.Id);
        }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<LoginUser> LoginUser { get; set; } = default!;
    }
}
