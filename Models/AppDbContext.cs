using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models;

namespace Nemesys.Data
{
    public class AppDbContext : IdentityDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StarRecord>()
                .HasKey(bc => new { bc.ReportId, bc.UserId });
            modelBuilder.Entity<StarRecord>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.StarredReports)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<StarRecord>()
                .HasOne(bc => bc.Report)
                .WithMany(c => c.UsersWhichHaveStarred)
                .HasForeignKey(bc => bc.ReportId);
        }

        public DbSet<StarRecord> StarRecords { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Investigation> Investigations { get; set; }
    }
}
