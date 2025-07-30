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
        public DbSet<Contractor> Contractors { get; set; }
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

                // FK for Contractor
                entity.HasOne(e => e.Contractor)
                      .WithMany()
                      .HasForeignKey(e => e.ContractorId)
                      .OnDelete(DeleteBehavior.SetNull);

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

            modelBuilder.Entity<Contractor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Trade).IsRequired();
                entity.Property(e => e.Rating);
                entity.Property(e => e.Availability);
                entity.Property(e => e.ContactInfo);
                entity.Property(e => e.Location);
                entity.Property(e => e.HourlyRate);
                entity.Property(e => e.Preferred);
                entity.Property(e => e.WarrantyApproved);
            });
        }
    }
}
