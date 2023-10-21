using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.Services.VehicleService
{
    public interface IVehicleService
    {
        public Task<BaseResponse<ListModel<Vehicle>>> GetVehicleListAsync(string? vehicleTypeNormaizeName, int pageNo = 1);
        public Task<BaseResponse<Vehicle>> GetVehicleByIdAsync(Guid id);
        public Task UpdateProductAsync(Guid id, Vehicle vehicle, IFormFile? formFile);
        public Task DeleteVehicle(Guid id);
        public Task<BaseResponse<Vehicle>> CreateVehicleAsync(Vehicle vehicle, IFormFile? formFile);
    }
}
