using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Web_153501_Kiselev.Entity;
using Web_153501_Kiselev.Services.CartService;
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
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddScoped(sp => SessionCart.GetCart(sp));

            /*            builder.Services.AddScoped<IVehicleTypeService,MemoryVehicleTypeService>();
                        builder.Services.AddScoped<IVehicleService, MemoryVehicleService>();*/

            builder.Services.AddHttpClient<IVehicleTypeService, ApiVehicleTypeService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
            builder.Services.AddHttpClient<IVehicleService, ApiVehicleService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
        }

        public static void AddAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = "cookie";
                opt.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("cookie")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
                    options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
                    options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];

                    // Получить Claims пользователя
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.ResponseType = "code";
                    options.ResponseMode = "query";
                    options.SaveTokens = true;
                });
        }
    }
}
