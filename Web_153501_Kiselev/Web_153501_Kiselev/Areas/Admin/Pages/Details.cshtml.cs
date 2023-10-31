using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Services.VehicleService;

namespace Web_153501_Kiselev.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IVehicleService _vehicleService;

        public DetailsModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public Vehicle Vehicle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _vehicleService.GetVehicleByIdAsync((Guid)id);

            if (!response.Success)
            {
                return NotFound();
            }
            else
            {
                Vehicle = response.Data;
            }
            return Page();
        }
    }
}
