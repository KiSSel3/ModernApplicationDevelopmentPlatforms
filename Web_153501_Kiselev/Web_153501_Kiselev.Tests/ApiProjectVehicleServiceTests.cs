using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_153501_Kiselev.API.Data;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.API.Services;
using Web_153501_Kiselev.Domain.Models;

namespace Web_153501_Kiselev.Tests
{
	public class ApiProjectVehicleServiceTests : IDisposable
	{
		private readonly DbConnection _connection;
		private readonly DbContextOptions<AppDbContext> _contextOptions;

		public void Dispose() => _connection.Dispose();

		private List<VehicleType> GetVehicleTypes()
		{
			return new List<VehicleType>() {
				new VehicleType() { Id = Guid.NewGuid()/*Guid.Parse("8de7288e-8b6d-4aa4-87da-6aede89ed857")*/, Name="Cедан", NormalizedName="sedan"},
				new VehicleType() { Id = Guid.NewGuid()/*Guid.Parse("737f5b38-079b-4680-a08b-82aa05191be5")*/, Name="Внедорожник", NormalizedName="suv"}
			};
		}

		private AppDbContext CreateContext() => new(_contextOptions);

		private List<Vehicle> GetVehicles()
		{
			return new List<Vehicle>()
			{
				new Vehicle()
				{
					Id = Guid.NewGuid()/*Guid.Parse("8de7288e-8b6d-4aa4-87da-6aede89ed858")*/,
					Model = "G 63 AMG",
					Description = "Хороший автомобиль",
					Price = 302000,
					Type = GetVehicleTypes().Find(item => item.NormalizedName.Equals("suv"))
				},
				new Vehicle()
				{
					Id = Guid.NewGuid()/* Guid.Parse("8de7288e-8b6d-4aa4-87da-6aede89ed859")*/,
					Model = "E 200 4M Sport",
					Description = "Хороший автомобиль",
					Price = 90000,
					Type = GetVehicleTypes().Find(item => item.NormalizedName.Equals("sedan"))
				},
				new Vehicle()
				{
					Id = Guid.NewGuid()/* Guid.Parse("8de7288e-8b6d-4aa4-87da-6aede89ed860")*/,
					Model = "1",
					Description = "Хороший автомобиль",
					Price = 302000,
					Type = GetVehicleTypes().Find(item => item.NormalizedName.Equals("suv"))
				},
				new Vehicle()
				{
					Id = Guid.NewGuid()/*Guid.Parse("8de7288e-8b6d-4aa4-87da-6aede89ed861")*/,
					Model = "2",
					Description = "Хороший автомобиль",
					Price = 90000,
					Type = GetVehicleTypes().Find(item => item.NormalizedName.Equals("sedan"))
				},
				new Vehicle()
				{
					Id = Guid.NewGuid()/* Guid.Parse("8de7288e-8b6d-4aa4-87da-6aede89ed860")*/,
					Model = "3",
					Description = "Хороший автомобиль",
					Price = 302000,
					Type = GetVehicleTypes().Find(item => item.NormalizedName.Equals("suv"))
				},
				new Vehicle()
				{
					Id = Guid.NewGuid()/*Guid.Parse("8de7288e-8b6d-4aa4-87da-6aede89ed861")*/,
					Model = "4",
					Description = "Хороший автомобиль",
					Price = 90000,
					Type = GetVehicleTypes().Find(item => item.NormalizedName.Equals("sedan"))
				},
				new Vehicle()
				{
					Id = Guid.NewGuid()/* Guid.Parse("8de7288e-8b6d-4aa4-87da-6aede89ed860")*/,
					Model = "5",
					Description = "Хороший автомобиль",
					Price = 302000,
					Type = GetVehicleTypes().Find(item => item.NormalizedName.Equals("suv"))
				},
				new Vehicle()
				{
					Id = Guid.NewGuid()/*Guid.Parse("8de7288e-8b6d-4aa4-87da-6aede89ed861")*/,
					Model = "6",
					Description = "Хороший автомобиль",
					Price = 90000,
					Type = GetVehicleTypes().Find(item => item.NormalizedName.Equals("sedan"))
				},
			};
		}

		public ApiProjectVehicleServiceTests()
		{
			_connection = new SqliteConnection("Filename=:memory:");
			_connection.Open();

			_contextOptions = new DbContextOptionsBuilder<AppDbContext>()
				.UseSqlite(_connection)
				.Options;

			using var context = new AppDbContext(_contextOptions);
			context.Database.EnsureCreated();
			context.Types.AddRange(GetVehicleTypes());
			context.Vehicles.AddRange(GetVehicles());
			context.SaveChanges();
		}

		[Fact]
		public void GetVehicleListAsync_ReturnsFirstPageWithThreeItems_WhenDefaultParametersPassed()
		{
			// Arrange
			using var context = CreateContext();
			VehicleService service = new(context, null!, null!);

			// Act
			var result = service.GetVehicleListAsync(null).Result;

			// Assert
			Assert.IsType<BaseResponse<ListModel<Vehicle>>>(result);
			Assert.True(result.Success);
			Assert.Equal(1, result.Data!.CurrentPage);
			Assert.Equal(3, result.Data.Items.Count);
			Assert.Equal(3, result.Data.TotalPages);
			Assert.Equal(context.Vehicles.First().Id, result.Data.Items[0].Id);
		}

		[Fact]
		public void GetVehicleListAsync_ReturnsSecondPageWithSecondThreeItems_WhenPageParameterEquals2()
		{
			// Arrange
			using var context = CreateContext();
			VehicleService service = new(context, null!, null!);

			// Act
			var result = service.GetVehicleListAsync(null, 2).Result;

			// Assert
			Assert.IsType<BaseResponse<ListModel<Vehicle>>>(result);
			Assert.True(result.Success);
			Assert.Equal(2, result.Data!.CurrentPage);
			Assert.Equal(3, result.Data.Items.Count);
			Assert.Equal(3, result.Data.TotalPages);
			Assert.Equal(context.Vehicles.Skip(3).First().Id, result.Data.Items.First().Id);
		}

		[Fact]
		public void GetVehicleListAsync_ReturnsValidVehicleByType_WhenTypeParameterPassed()
		{
			// Arrange
			using var context = CreateContext();
			VehicleService service = new(context, null!, null!);

			// Act
			var result = service.GetVehicleListAsync("suv").Result;

			// Assert
			Assert.IsType<BaseResponse<ListModel<Vehicle>>>(result);
			Assert.True(result.Success);
			Assert.Equal(1, result.Data!.CurrentPage);
			Assert.Equal(3, result.Data.Items.Count);
			Assert.Equal(2, result.Data.TotalPages);
			Assert.DoesNotContain(result.Data.Items, x => x.Type.Id == GetVehicleTypes().Find(item => item.NormalizedName.Equals("suv")).Id);
		}

		[Fact]
		public void GetVehicleListAsync_ReturnsSuccessFalse_WhenPageNumberParameterIsGreaterThanTotalPages()
		{
			// Arrange
			using var context = CreateContext();
			VehicleService service = new(context, null!, null!);

			// Act
			var result = service.GetVehicleListAsync(null, 1000).Result;

			// Assert
			Assert.IsType<BaseResponse<ListModel<Vehicle>>>(result);
			Assert.False(result.Success);
		}
	}
}
