using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Services.VehicleService;

namespace Web_153501_Kiselev.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IVehicleService _vehicleService;

        public EditModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [BindProperty]
        public Vehicle Vehicle { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _vehicleService.UpdateProductAsync(Vehicle.Id, Vehicle, Image);

            return RedirectToPage("./Index");
        }
    }
}
