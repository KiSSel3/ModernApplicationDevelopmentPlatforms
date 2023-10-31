using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Services.VehicleService;
using Web_153501_Kiselev.Services.VehicleTypeService;

namespace Web_153501_Kiselev.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleTypeService _vehicleTypeService;

        public CreateModel(IVehicleService vehicleService, IVehicleTypeService vehicleTypeService)
        {
            _vehicleService = vehicleService;
            _vehicleTypeService = vehicleTypeService;
        }

        public async Task<IActionResult> OnGet()
        {
            var types = await _vehicleTypeService.GetVehicleTypeListAsync();

            if (types.Success)
            {
                ViewData["VehicleTypes"] = new SelectList(types.Data, "Id", "Name");

                return Page();
            }

            return NotFound();
        }

        [BindProperty]
        public Vehicle Vehicle { get; set; } = default!;

        [BindProperty]
        public IFormFile Image { get; set; }

        [BindProperty]
        public Guid TypeId { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var types = await _vehicleTypeService.GetVehicleTypeListAsync();

            if (!ModelState.IsValid || Vehicle == null || TypeId == null)
            {
                ViewData["VehicleTypes"] = new SelectList(types.Data, "Id", "Name");

                return Page();
            }

            var vehicleType = types.Data.FirstOrDefault(t => t.Id.Equals(TypeId));

            if (vehicleType == null)
            {
                ViewData["VehicleTypes"] = new SelectList(types.Data, "Id", "Name");

                return Page();
            }

            Vehicle.Type = vehicleType;

            await _vehicleService.CreateVehicleAsync(Vehicle, Image);

            return RedirectToPage("./Index");
        }
    }
}
