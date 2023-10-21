using Web_153501_Kiselev.Services.VehicleService;
using Web_153501_Kiselev.Services.VehicleTypeService;

namespace Web_153501_Kiselev
{
    public static class BuilderExtension
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IVehicleTypeService,MemoryVehicleTypeService>();
            builder.Services.AddScoped<IVehicleService, MemoryVehicleService>();
        }
    }
}
