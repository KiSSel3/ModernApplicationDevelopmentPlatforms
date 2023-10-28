using Microsoft.AspNetCore.Mvc;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;
using Web_153501_Kiselev.Services.VehicleService;
using Web_153501_Kiselev.Services.VehicleTypeService;

namespace Web_153501_Kiselev.Controllers
{
    public class VehicleController : Controller
    {
        private IVehicleTypeService _vehicleTypeService;
        private IVehicleService _vehicleService;

        public VehicleController(IVehicleTypeService vehicleTypeService, IVehicleService vehicleService) => (_vehicleTypeService, _vehicleService) = (vehicleTypeService, vehicleService);

        public async Task<IActionResult> Index(string? type, int pageNo = 1)
        {
            BaseResponse<ListModel<Vehicle>> responseVehicle = await _vehicleService.GetVehicleListAsync(type, pageNo);
            BaseResponse<List<VehicleType>> responseType = await _vehicleTypeService.GetVehicleTypeListAsync();

            if (!responseType.Success)
                return NotFound(responseType.Message);

            if (!responseVehicle.Success)
                return NotFound(responseVehicle.Message);

            ViewData["typeList"] = responseType.Data;
            ViewData["currentType"] = type;

            return View(responseVehicle.Data);
        }
    }
}
