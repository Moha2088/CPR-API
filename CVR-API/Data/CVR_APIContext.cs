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
        public CVR_APIContext(DbContextOptions<CVR_APIContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(p => p.Id);

                builder.HasIndex(p => p.Id)
                .IsUnique();

                builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(10);
            });
        }

        public DbSet<User> User { get; set; } = default!;
    }
}
