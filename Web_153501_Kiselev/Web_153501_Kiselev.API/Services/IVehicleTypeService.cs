using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.API.Services
{
    public interface IVehicleTypeService
    {
        public Task<BaseResponse<List<VehicleType>>> GetVehicleTypeListAsync();
    }
}
