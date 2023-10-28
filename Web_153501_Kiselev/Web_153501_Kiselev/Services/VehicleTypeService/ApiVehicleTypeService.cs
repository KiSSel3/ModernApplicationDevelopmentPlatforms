using System.Text;
using System.Text.Json;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.Services.VehicleTypeService
{
    public class ApiVehicleTypeService : IVehicleTypeService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger _logger;

        public ApiVehicleTypeService(HttpClient httpClient, ILogger<ApiVehicleTypeService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _serializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }
        public async Task<BaseResponse<List<VehicleType>>> GetVehicleTypeListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}VehicleTypes/");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<BaseResponse<List<VehicleType>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");

                    return new BaseResponse<List<VehicleType>>(false, ex.Message);
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");

            return new BaseResponse<List<VehicleType>>(false, $"Данные не получены от сервера. Error:{response.StatusCode}");
        }
    }
}
