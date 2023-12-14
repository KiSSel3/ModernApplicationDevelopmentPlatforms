using Web_153501_Kiselev.Domain.Entities;

namespace Web_153501_Kiselev.BlazorWasm.Services
{
	public interface IDataService
	{
		event Action DataChanged;
		List<VehicleType>? Types { get; set; }
		List<Vehicle>? VehicleList { get; set; }
		bool Success { get; set; }
		string? ErrorMessage { get; set; }
		int TotalPages { get; set; }
		int CurrentPage { get; set; }

		public Task GetVehicleListAsync(string? typeNormalizedName, int pageNo = 1);

		public Task<Vehicle?> GetVehicleByIdAsync(int id);

		public Task GetTypeListAsync();
	}
}
