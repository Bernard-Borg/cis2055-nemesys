using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models;

namespace Nemesys.Models
{
    public class AppDbContext : IdentityDbContext<User>
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

            //Many-to-many relationships need to be manually defined
            modelBuilder.Entity<StarRecord>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.StarredReports)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<StarRecord>()
                .HasOne(bc => bc.Report)
                .WithMany(c => c.UsersWhichHaveStarred)
                .HasForeignKey(bc => bc.ReportId);

            //"Dependent" needed to be defined
            modelBuilder.Entity<Report>()
                .HasOne(i => i.Investigation)
                .WithOne(r => r.Report)
                .HasForeignKey<Investigation>(i => i.ReportId);
        }

        public DbSet<StarRecord> StarRecords { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Investigation> Investigations { get; set; }
        public DbSet<HazardType> HazardTypes { get; set; }
        public DbSet<ReportStatus> ReportStatuses { get; set; }
    }
}
