using JobSage.Domain.Entities;
using JobSage.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobSage.Infrastructure.Persistence
{
    public class JobSageDbContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<PropertyInformation> Properties { get; set; }
        public DbSet<SchedulingInfo> SchedulingInfos { get; set; }
        public JobSageDbContext(DbContextOptions<JobSageDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(e => e.Id);
                // FK for PropertyInformation
                entity.HasOne(e => e.PropertyInfo)
                      .WithMany()
                      .HasForeignKey(e => e.PropertyInfoId)
                      .OnDelete(DeleteBehavior.Cascade);

                // FK for SchedulingInfo
                entity.HasOne(e => e.Scheduling)
                      .WithMany()
                      .HasForeignKey(e => e.SchedulingId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.OwnsOne(e => e.Cost);
            });
            modelBuilder.Entity<PropertyInformation>(entity =>
            {
                entity.HasKey(e => e.PropertyId);
                entity.Property(e => e.PropertyAddress).IsRequired();
            });
            modelBuilder.Entity<SchedulingInfo>(entity =>
            {
                entity.HasKey(e => e.SchedulingId);
                entity.Property(e => e.ScheduledDate).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });
        }
    }
}
