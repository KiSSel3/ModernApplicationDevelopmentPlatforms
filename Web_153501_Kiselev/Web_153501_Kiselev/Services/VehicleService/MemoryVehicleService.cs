using Microsoft.AspNetCore.Mvc;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;
using Web_153501_Kiselev.Services.VehicleTypeService;

namespace Web_153501_Kiselev.Services.VehicleService
{
    public class MemoryVehicleService : IVehicleService
    {
        private IConfiguration _config;

        private ListModel<Vehicle> _vehicle;
        private List<VehicleType> _vehicleTypes;

        public MemoryVehicleService([FromServices] IConfiguration config, IVehicleTypeService vehicleTypeService)
        {
            _vehicleTypes = vehicleTypeService.GetVehicleTypeListAsync()
                                              .Result
                                              .Data;
            _config = config;

            SetupData();
        }

        private void SetupData()
        {
            List<Vehicle> list = new List<Vehicle>()
            {
                new Vehicle() { Id = Guid.NewGuid(), Model = "E 200 4M Sport", Description = "Хороший автомобиль",
                    Price = 90000, ImagePath = "images/E.jpg", Type = _vehicleTypes.Find(item => item.NormalizedName.Equals("sedan")) },

                new Vehicle() { Id = Guid.NewGuid(), Model = "G 63 AMG", Description = "Хороший автомобиль",
                    Price = 302000, ImagePath = "images/G63.jpg", Type = _vehicleTypes.Find(item => item.NormalizedName.Equals("suv")) },

                new Vehicle() { Id = Guid.NewGuid(), Model = "SL", Description = "Хороший автомобиль",
                    Price = 240000, ImagePath = "images/SL.jpg", Type = _vehicleTypes.Find(item => item.NormalizedName.Equals("convertible")) },

                new Vehicle() { Id = Guid.NewGuid(), Model = "AMG GT", Description = "Хороший автомобиль",
                    Price = 210000, ImagePath = "images/AMG_GT.jpeg", Type = _vehicleTypes.Find(item => item.NormalizedName.Equals("sedan")) },
            };

            _vehicle = new ListModel<Vehicle>() { Items = list };
        }

        public Task<BaseResponse<Vehicle>> CreateVehicleAsync(Vehicle vehicle, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteVehicle(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<Vehicle>> GetVehicleByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ListModel<Vehicle>>> GetVehicleListAsync(string? vehicleTypeNormaizeName, int pageNo = 1)
        {
            List<Vehicle> responseVehicle = null; 

            int itemsPerPage = int.Parse(_config.GetSection("ItemsPerPage").Value);


            if(vehicleTypeNormaizeName != null)
            {
                responseVehicle = _vehicle.Items.Where(v => vehicleTypeNormaizeName == null || v.Type.NormalizedName.Equals(vehicleTypeNormaizeName)).ToList();
            }
            else
            {
                responseVehicle = _vehicle.Items.ToList();
            }

            BaseResponse<ListModel<Vehicle>> result = new BaseResponse<ListModel<Vehicle>>(true, new ListModel<Vehicle>()
            {
                Items = responseVehicle.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList(),
                CurrentPage = pageNo,
                TotalPages = Convert.ToInt32(Math.Ceiling((double)responseVehicle.Count / itemsPerPage))
            });

            return Task.FromResult(result);
        }

        public Task UpdateProductAsync(Guid id, Vehicle vehicle, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
