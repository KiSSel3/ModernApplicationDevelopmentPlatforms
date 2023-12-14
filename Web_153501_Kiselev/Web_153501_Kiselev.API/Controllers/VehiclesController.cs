using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_153501_Kiselev.API.Services;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        private readonly string _imagesPath;
        private readonly string _appUri;

        public VehiclesController(IVehicleService vehicleService, IWebHostEnvironment env, IConfiguration configuration)
        {
            _vehicleService = vehicleService;
            _env = env;
            _configuration = configuration;

            _imagesPath = Path.Combine(_env.WebRootPath, "images");
            _appUri = _configuration.GetSection("appUri").Value;
        }

		// GET: api/Vehicles
		[HttpGet("")]
        [Authorize]
        [Route("{type:alpha}")]
        [Route("page{pageNo}")]
        [Route("{type}/page{pageNo}")]
        public async Task<ActionResult<BaseResponse<List<Vehicle>>>> GetVehicles(string? type, int pageNo = 1, int pageSize = 3)
        {
            var response = await _vehicleService.GetVehicleListAsync(type, pageNo, pageSize);

            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound();
        }

		// GET: api/Vehicles/5
		[HttpGet("{id}")]
		[Authorize]
		public async Task<ActionResult<BaseResponse<Vehicle>>> GetVehicle(Guid id)
        {
            var response = await _vehicleService.GetVehicleByIdAsync(id);

            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound();
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(Guid id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest();
            }

            await _vehicleService.UpdateVehicleAsync(id, vehicle);

            return NoContent();
        }

        // POST: api/Vehicles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BaseResponse<Vehicle>>> PostVehicle(Vehicle vehicle)
        {
            var response = await _vehicleService.CreateVehicleAsync(vehicle);

            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound();
        }

        // DELETE: api/Vehicles/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task DeleteVehicle(Guid id)
        {
            await _vehicleService.DeleteVehicle(id);
        }

        [Authorize]
        [HttpPost("{id}")]
        public async Task<ActionResult<BaseResponse<string>>> PostImage(Guid id, IFormFile formFile)
        {
            var response = await _vehicleService.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
