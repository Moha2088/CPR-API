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

        public DbSet<CVR_API.Models.User> User { get; set; } = default!;
    }
}
