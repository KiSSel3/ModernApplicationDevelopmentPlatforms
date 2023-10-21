using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.Services.VehicleTypeService
{
    public class MemoryVehicleTypeService : IVehicleTypeService
    {
        public Task<BaseResponse<ListModel<VehicleType>>> GetVehicleTypeListAsync()
        {
            List<VehicleType> vehicleTypes = new List<VehicleType>()
            {
                new VehicleType() {Id=Guid.NewGuid(), Name="Седан", NormalizedName="sedan"},
                new VehicleType() {Id=Guid.NewGuid(), Name="Кабриолет", NormalizedName="convertible"},
                new VehicleType() {Id=Guid.NewGuid(), Name="Внедорожник", NormalizedName="suv"},
            };

            BaseResponse<ListModel<VehicleType>> result = new BaseResponse<ListModel<VehicleType>>(true, new ListModel<VehicleType>() { Items = vehicleTypes});

            return Task.FromResult(result);
        }
    }
}
