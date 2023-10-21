using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.Services.VehicleTypeService
{
    public interface IVehicleTypeService
    {
        public Task<BaseResponse<ListModel<VehicleType>>> GetVehicleTypeListAsync();
    }
}
