using Microsoft.EntityFrameworkCore;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //await db.Database.MigrateAsync();

            await db.Types.AddAsync(new VehicleType() { Name = "Седан", NormalizedName = "sedan" });
            await db.Types.AddAsync(new VehicleType() { Name = "Кабриолет", NormalizedName = "convertible" });
            await db.Types.AddAsync(new VehicleType() { Name = "Внедорожник", NormalizedName = "suv" });

            await db.SaveChangesAsync();

            await db.Vehicles.AddAsync(new Vehicle()
            {
                Model = "E 200 4M Sport",
                Description = "Хороший автомобиль",
                Price = 90000,
                ImagePath = app.Configuration.GetValue<string>("ImageUrl") + "images/E.jpg",
                Type = db.Types.ToList().Find(item => item.NormalizedName.Equals("sedan"))
            });

            await db.Vehicles.AddAsync(new Vehicle()
            {
                Model = "G 63 AMG",
                Description = "Хороший автомобиль",
                Price = 302000,
                ImagePath = app.Configuration.GetValue<string>("ImageUrl") + "images/G63.jpg",
                Type = db.Types.ToList().Find(item => item.NormalizedName.Equals("suv"))
            });

            await db.Vehicles.AddAsync(new Vehicle()
            {
                Id = Guid.NewGuid(),
                Model = "SL",
                Description = "Хороший автомобиль",
                Price = 240000,
                ImagePath = app.Configuration.GetValue<string>("ImageUrl") + "images/SL.jpg",
                Type = db.Types.ToList().Find(item => item.NormalizedName.Equals("convertible"))
            });

            await db.Vehicles.AddAsync(new Vehicle()
            {
                Id = Guid.NewGuid(),
                Model = "AMG GT",
                Description = "Хороший автомобиль",
                Price = 210000,
                ImagePath = app.Configuration.GetValue<string>("ImageUrl") + "images/AMG_GT.jpeg",
                Type = db.Types.ToList().Find(item => item.NormalizedName.Equals("sedan"))
            });

            await db.SaveChangesAsync();
        }
    }
}
