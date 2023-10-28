using Microsoft.EntityFrameworkCore;
using Web_153501_Kiselev.API.Data;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.API.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly AppDbContext _db;
        private readonly int _maxPageSize = 20;

        public VehicleService(AppDbContext db) => (_db) = (db);

        public async Task<BaseResponse<Vehicle>> CreateVehicleAsync(Vehicle vehicle)
        {
            try
            {
                await _db.Vehicles.AddAsync(vehicle);
                await _db.SaveChangesAsync();

                return new BaseResponse<Vehicle>(true, vehicle);
            }
            catch (Exception ex)
            {
                return new BaseResponse<Vehicle>(false, ex.Message);
            }
        }

        public async Task DeleteVehicle(Guid id)
        {
            var removeItem = await _db.Vehicles.FirstOrDefaultAsync(item => item.Id.Equals(id));

            if (removeItem != null)
            {
                _db.Vehicles.Remove(removeItem);

                await _db.SaveChangesAsync();
            }
        }

        public async Task<BaseResponse<Vehicle>> GetVehicleByIdAsync(Guid id)
        {
            try
            {
                var searchVehicle = await _db.Vehicles
                    .AsNoTracking()
                    .Include(v => v.Type)
                    .FirstOrDefaultAsync(item => item.Id.Equals(id));

                if(searchVehicle != null)
                {
                    return new BaseResponse<Vehicle>(true, searchVehicle);
                }

                return new BaseResponse<Vehicle>(false, "Not Found");
            }
            catch (Exception ex)
            {
                return new BaseResponse<Vehicle>(false, ex.Message);
            }
        }

        public async Task<BaseResponse<ListModel<Vehicle>>> GetVehicleListAsync(string? vehicleTypeNormaizeName, int pageNo = 1, int pageSize = 3)
        {
            try
            {
                if (pageSize > _maxPageSize)
                    pageSize = _maxPageSize;

                var dataList = new ListModel<Vehicle>();

                var query = _db.Vehicles.AsQueryable()
                                        .AsNoTracking()
                                        .Include(v => v.Type)
                                        .Where(d => vehicleTypeNormaizeName == null || d.Type.NormalizedName.Equals(vehicleTypeNormaizeName));

                var count = await query.CountAsync();
                if (count == 0)
                {
                    return new BaseResponse<ListModel<Vehicle>>(true, dataList);
                }

                int totalPages = (int)Math.Ceiling(count / (double)pageSize);
                if (pageNo > totalPages)
                {
                    return new BaseResponse<ListModel<Vehicle>>(false, "No such page");
                }

                dataList.Items = await query
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                dataList.CurrentPage = pageNo;
                dataList.TotalPages = totalPages;

                return new BaseResponse<ListModel<Vehicle>>(true, dataList);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ListModel<Vehicle>>(false, ex.Message);
            }
        }

        public Task<BaseResponse<string>> SaveImageAsync(Guid id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(Guid id, Vehicle vehicle)
        {
            throw new NotImplementedException();
        }
    }
}
