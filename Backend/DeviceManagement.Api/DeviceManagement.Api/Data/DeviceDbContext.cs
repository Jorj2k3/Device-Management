using DeviceManagement.Api.Models;
using Microsoft.EntityFrameworkCore;


namespace DeviceManagement.Api.Data
{
    /// <summary>
    /// Represents the database context for the Device Management API.
    /// </summary>
    public class DeviceDbContext : DbContext
    {
        public DeviceDbContext(DbContextOptions<DeviceDbContext> options) : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Device>().ToTable("Devices");

            modelBuilder.Entity<Device>()
                .HasOne(d => d.AssignedUser)
                .WithMany(u => u.Devices)
                .HasForeignKey(d => d.AssignedUserID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
