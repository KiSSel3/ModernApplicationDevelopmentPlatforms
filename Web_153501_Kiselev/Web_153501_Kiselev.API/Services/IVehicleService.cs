using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.API.Services
{
    public interface IVehicleService
    {
        public Task<BaseResponse<ListModel<Vehicle>>> GetVehicleListAsync(string? vehicleTypeNormaizeName, int pageNo = 1, int pageSize = 3);
        public Task<BaseResponse<Vehicle>> GetVehicleByIdAsync(Guid id);
        public Task UpdateVehicleAsync(Guid id, Vehicle vehicle);
        public Task DeleteVehicle(Guid id);
        public Task<BaseResponse<Vehicle>> CreateVehicleAsync(Vehicle vehicle);
        public Task<BaseResponse<string>> SaveImageAsync(Guid id, IFormFile formFile);
    }
}
