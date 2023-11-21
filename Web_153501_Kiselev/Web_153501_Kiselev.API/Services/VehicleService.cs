using Microsoft.EntityFrameworkCore;
using Web_153501_Kiselev.API.Data;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.API.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _maxPageSize = 20;

        public VehicleService(AppDbContext db, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) => (_db, _env, _httpContextAccessor) = (db, env, httpContextAccessor);

        public async Task<BaseResponse<Vehicle>> CreateVehicleAsync(Vehicle vehicle)
        {
            try
            {
                if(vehicle != null && vehicle.Type != null)
                {
                    vehicle.Type = await _db.Types.FirstOrDefaultAsync(t => t.Id.Equals(vehicle.Type.Id));
                }

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
            var query = _db.Vehicles.AsQueryable();

            if(true)
            {
                query.Include(v => v.Type);
            }

            try
            {
                var searchVehicle = await query
                    .AsNoTracking()
                    
                    .FirstOrDefaultAsync(item => item.Id.Equals(id));

                if (searchVehicle != null)
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

        public async Task<BaseResponse<string>> SaveImageAsync(Guid id, IFormFile formFile)
        {
            var vehicle = await _db.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return new BaseResponse<string>(false, "No item found");
            }

            var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;

            var imageFolder = Path.Combine(_env.WebRootPath, "images");

            if (formFile != null)
            {
                if (!String.IsNullOrEmpty(vehicle.ImagePath))
                {
                    var prevImage = Path.Combine(imageFolder, Path.GetFileName(vehicle.ImagePath));
                    if (File.Exists(prevImage))
                    {
                        File.Delete(prevImage);
                    }
                }

                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                var fPath = Path.Combine(imageFolder, fName);

                using (var stream = new FileStream(fPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                vehicle.ImagePath = $"{host}/images/{fName}";
                await _db.SaveChangesAsync();
            }

            return new BaseResponse<string>(true, data: vehicle.ImagePath);
        }

        public async Task UpdateVehicleAsync(Guid id, Vehicle vehicle)
        {
            _db.Vehicles.Update(vehicle);
            await _db.SaveChangesAsync();
        }
    }
}
