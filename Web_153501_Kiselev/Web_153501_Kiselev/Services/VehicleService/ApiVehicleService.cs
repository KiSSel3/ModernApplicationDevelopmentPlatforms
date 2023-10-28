using System.Data.SqlTypes;
using System.Text;
using System.Text.Json;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.Services.VehicleService
{
    public class ApiVehicleService : IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        private readonly string _itemsPerPage;

        public ApiVehicleService(HttpClient httpClient, ILogger<ApiVehicleService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;

            _serializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            _itemsPerPage = _configuration.GetSection("ItemsPerPage").Value;
        }

        public async Task<BaseResponse<Vehicle>> CreateVehicleAsync(Vehicle vehicle, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Vehicles");

            var response = await _httpClient.PostAsJsonAsync(uri, vehicle, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<BaseResponse<Vehicle>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");

                    return new BaseResponse<Vehicle>(false, ex.Message);
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");

            return new BaseResponse<Vehicle>(false, $"Данные не получены от сервера. Error:{response.StatusCode}");
        }

        public async Task DeleteVehicle(Guid id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Vehicles/{id}");

            await _httpClient.DeleteAsync(uri);
        }

        public async Task<BaseResponse<Vehicle>> GetVehicleByIdAsync(Guid id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Vehicles/{id}");

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<BaseResponse<Vehicle>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");

                    return new BaseResponse<Vehicle>(false, ex.Message);
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode.ToString()}");

            return new BaseResponse<Vehicle>(false, $"Данные не получены от сервера. Error:{response.StatusCode}");
        }

        public async Task<BaseResponse<ListModel<Vehicle>>> GetVehicleListAsync(string? vehicleTypeNormaizeName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Vehicles/");

            if (vehicleTypeNormaizeName != null)
            {
                urlString.Append($"{vehicleTypeNormaizeName}/");
            }

            if (pageNo > 1)
            {
                urlString.Append($"page{pageNo}");
            };

            if (!_itemsPerPage.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _itemsPerPage));
            }

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<BaseResponse<ListModel<Vehicle>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");

                    return new BaseResponse<ListModel<Vehicle>>(false, ex.Message);
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{ response.StatusCode}");

            return new BaseResponse<ListModel<Vehicle>>(false, $"Данные не получены от сервера. Error:{response.StatusCode}");
        }

        public Task UpdateProductAsync(Guid id, Vehicle vehicle, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
