using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Services.VehicleService;

namespace Web_153501_Kiselev.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IVehicleService _vehicleService;

        public IndexModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public IList<Vehicle> Vehicle { get; set; } = default!;

        public async Task OnGetAsync(int pageNo = 1)
        {
            var response = await _vehicleService.GetVehicleListAsync(null, pageNo);
            if (response.Success)
            {
                Vehicle = response.Data.Items;
            }
        }
    }
}
