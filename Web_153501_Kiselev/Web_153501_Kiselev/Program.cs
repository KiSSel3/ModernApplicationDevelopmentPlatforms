using Web_153501_Kiselev;
using Serilog;
using Web_153501_Kiselev.Middleware;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.AddServices();
        builder.AddAuthentication();

        builder.Services.AddRazorPages();

		var logger = new LoggerConfiguration()
	                        .ReadFrom.Configuration(builder.Configuration)
	                        .CreateLogger();

		var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages().RequireAuthorization();

		app.UseMiddleware<LoggingMiddleware>(logger);

		app.UseSession();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}