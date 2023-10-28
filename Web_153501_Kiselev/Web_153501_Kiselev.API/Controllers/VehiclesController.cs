using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_153501_Kiselev.API.Data;
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

        public VehiclesController(IVehicleService vehicleService) => (_vehicleService) = (vehicleService);

        // GET: api/Vehicles
        [HttpGet("")]
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
        public async Task<ActionResult<BaseResponse<Vehicle>>> GetVehicle(Guid id)
        {
            var response = await _vehicleService.GetVehicleByIdAsync(id);

            if (response.Success)
            {
                return Ok(response);
            }
            
            return NotFound();
        }

/*        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(Guid id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Vehicles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
        [HttpDelete("{id}")]
        public async Task DeleteVehicle(Guid id)
        {
            await _vehicleService.DeleteVehicle(id);
        }
    }
}
