using Web_153501_Kiselev.Entity;
using Web_153501_Kiselev.Services.VehicleService;
using Web_153501_Kiselev.Services.VehicleTypeService;

namespace Web_153501_Kiselev
{
    public static class BuilderExtension
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            UriData uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddLogging();

/*            builder.Services.AddScoped<IVehicleTypeService,MemoryVehicleTypeService>();
            builder.Services.AddScoped<IVehicleService, MemoryVehicleService>();*/

            builder.Services.AddHttpClient<IVehicleTypeService, ApiVehicleTypeService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
            builder.Services.AddHttpClient<IVehicleService, ApiVehicleService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
        }
    }
}
