using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealStats.Models;

namespace RealStats.Data
{
    public class RealStateContext : IdentityDbContext<ApplicationUser>
    {
        public RealStateContext(DbContextOptions<RealStateContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ReportIssue>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();
            
            builder.Entity<Feature>().HasData(
                new Feature {Id = 1 ,  Name = "Swimming Pool" },
                new Feature {Id = 2 ,  Name = "Laundry Room"},
                new Feature {Id = 3 ,  Name = "Emergency Exit"},
                new Feature {Id = 4 ,  Name = "Fire Place"},
                new Feature {Id = 5 ,  Name = "Garden"},
                new Feature {Id = 6 ,  Name = "Smart Home"},
                new Feature {Id = 7 ,  Name = "Elevator Access" }
            );
        }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<Properity> Properities { get; set; }
        public DbSet<LeaseAgreement> LeaseAgreement { get; set; }
        public DbSet<ReportIssue> ReportIssues { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<TermsAndConditions> TermsAndConditions { get; set; }
    }
    
}
