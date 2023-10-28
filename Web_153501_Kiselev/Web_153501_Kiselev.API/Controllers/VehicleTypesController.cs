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
    public class VehicleTypesController : ControllerBase
    {
        private readonly IVehicleTypeService _vehicleTypeService;

        public VehicleTypesController(IVehicleTypeService vehicleTypeService) => (_vehicleTypeService) = (vehicleTypeService);

        // GET: api/VehicleTypes
        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<VehicleType>>>> GetTypes()
        {
            var response = await _vehicleTypeService.GetVehicleTypeListAsync();

            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound();
        }
    }
}
