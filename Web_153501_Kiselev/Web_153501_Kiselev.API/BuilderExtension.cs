using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Web_153501_Kiselev.API.Data;
using Web_153501_Kiselev.API.Services;

namespace Web_153501_Kiselev.API
{
    public static class BuilderExtension
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();

            builder.Services
                        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(opt =>
                        {
                            opt.Authority = builder
                            .Configuration
                            .GetSection("isUri").Value;
                            opt.TokenValidationParameters.ValidateAudience = false;
                            opt.TokenValidationParameters.ValidTypes =
                            new[] { "at+jwt" };
                        });

/*			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin",
					builder => builder.WithOrigins("https://localhost:7288")
					.AllowAnyHeader()
					.AllowAnyMethod());
			});*/

			builder.Services.AddScoped<IVehicleTypeService, VehicleTypeService>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();
        }

        public static void AddDataBase(this WebApplicationBuilder builder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        }
    }
}
