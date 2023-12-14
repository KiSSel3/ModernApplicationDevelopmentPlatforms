using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.BlazorWasm.Services
{
	public class DataService : IDataService
	{
		public event Action DataChanged;
		private readonly HttpClient _httpClient;
		private readonly IAccessTokenProvider _accessTokenProvider;
		private readonly ILogger<DataService> _logger;
		private readonly int _pageSize = 3;
		private readonly JsonSerializerOptions _jsonSerializerOptions;

		public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider accessTokenProvider, ILogger<DataService> logger)
		{
			_httpClient = httpClient;
			_pageSize = configuration.GetSection("PageSize").Get<int>();
			_jsonSerializerOptions = new JsonSerializerOptions()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
			_accessTokenProvider = accessTokenProvider;
			_logger = logger;
		}

		public List<VehicleType>? Types { get; set; }
		public List<Vehicle>? VehicleList { get; set; }
		public bool Success { get; set; }
		public string? ErrorMessage { get; set; }
		public int TotalPages { get; set; }
		public int CurrentPage { get; set; }

		public async Task GetTypeListAsync()
		{
			var tokenRequest = await _accessTokenProvider.RequestAccessToken();
			if (tokenRequest.TryGetToken(out var token))
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

				var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}api/VehicleTypes/");
				var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
				if (response.IsSuccessStatusCode)
				{
					try
					{
						var responseData = await response.Content.ReadFromJsonAsync<BaseResponse<List<VehicleType>>>(_jsonSerializerOptions);
						Types = responseData?.Data;
						Success = true;
					}
					catch (JsonException ex)
					{
						Success = false;
						ErrorMessage = $"Ошибка: {ex.Message}";
					}
				}
				else
				{
					Success = false;
					ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
				}
			}
		}

		public async Task<Vehicle?> GetVehicleByIdAsync(int id)
		{
			var tokenRequest = await _accessTokenProvider.RequestAccessToken();
			if (tokenRequest.TryGetToken(out var token))
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

				var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}api/Vehicles/{id}");
				var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

				if (response.IsSuccessStatusCode)
				{
					try
					{
						Success = true;
						return (await response.Content.ReadFromJsonAsync<BaseResponse<Vehicle>>(_jsonSerializerOptions))?.Data;
					}
					catch (JsonException ex)
					{
						Success = false;
						ErrorMessage = $"Ошибка: {ex.Message}";
						return null;
					}
				}
				Success = false;
				ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
			}

			return null;
		}

		public async Task GetVehicleListAsync(string? typeNormalizedName, int pageNo = 1)
		{
			var tokenRequest = await _accessTokenProvider.RequestAccessToken();
			if (tokenRequest.TryGetToken(out var token))
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

				var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}api/Vehicles/");
				if (typeNormalizedName != null)
				{
					urlString.Append($"{typeNormalizedName}/");
				};
				if (pageNo > 1)
				{
					urlString.Append($"page{pageNo}");
				};
				if (!_pageSize.Equals("3"))
				{
					urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));
				}

				var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
				if (response.IsSuccessStatusCode)
				{
					try
					{
						var responseData = await response.Content.ReadFromJsonAsync<BaseResponse<ListModel<Vehicle>>>(_jsonSerializerOptions);
						VehicleList = responseData?.Data?.Items;
						TotalPages = responseData?.Data?.TotalPages ?? 0;
						CurrentPage = responseData?.Data?.CurrentPage ?? 0;
						DataChanged?.Invoke();
						_logger.LogInformation("<------ Vehicle list received successfully ------>");
						Success = true;
					}
					catch (JsonException ex)
					{
						Success = false;
						ErrorMessage = $"Ошибка: {ex.Message}";
					}
				}
				else
				{
					Success = false;
					ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
				}
			}
		}
	}
}
