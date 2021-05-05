using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CidadeAlta_CodigoPenal.Models
{
    public class CriminalCodeContext : DbContext { 
    
        public DbSet<CriminalCode> CriminalCodes { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<User> Users { get; set; }

        public CriminalCodeContext(DbContextOptions<CriminalCodeContext> options) : base(options)
        {
        }
    }
}
