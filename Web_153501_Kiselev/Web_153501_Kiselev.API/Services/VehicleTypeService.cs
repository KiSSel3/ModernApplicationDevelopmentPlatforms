using Microsoft.EntityFrameworkCore;
using Web_153501_Kiselev.API.Data;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.API.Services
{
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly AppDbContext _db;
        public VehicleTypeService(AppDbContext db) => (_db) = (db);

        public async Task<BaseResponse<List<VehicleType>>> GetVehicleTypeListAsync()
        {
            try
            {
                var vehicleTypes = await _db.Types.ToListAsync();

                return new BaseResponse<List<VehicleType>>(true, vehicleTypes);
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<VehicleType>>(false, ex.Message);
            }
        }
    }
}
