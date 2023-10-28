using Microsoft.EntityFrameworkCore;
using Web_153501_Kiselev.Domain.Entities;

namespace Web_153501_Kiselev.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; } = null!; 
        public DbSet<VehicleType> Types { get; set; } = null!; 

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
