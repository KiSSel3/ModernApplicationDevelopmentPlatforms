using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_153501_Kiselev.Domain.Models;
using Web_153501_Kiselev.Services.VehicleService;

namespace Web_153501_Kiselev.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly Cart _cart;

        public CartController(IVehicleService vehicleService, Cart cart)
        {
            _vehicleService = vehicleService;
            _cart = cart;
        }

        public IActionResult Index()
        {
            return View(_cart);
        }

        [Route("[controller]/add/{id:Guid}")]
        public async Task<ActionResult> Add(Guid id, string returnUrl)
        {
            var data = await _vehicleService.GetVehicleByIdAsync(id);
            if (data.Success)
            {
                _cart.AddToCart(data.Data);
            }
            return Redirect(returnUrl);
        }

        [Route("[controller]/remove/{id:Guid}")]
        public async Task<ActionResult> Remove(Guid id, string returnUrl)
        {
            var data = await _vehicleService.GetVehicleByIdAsync(id);
            if (data.Success)
            {
                _cart.RemoveItems(data.Data.Id);
            }
            return Redirect(returnUrl);
        }
    }
}
