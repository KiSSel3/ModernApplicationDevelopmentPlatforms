using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using Web_153501_Kiselev.Services;
using Web_153501_Kiselev.Controllers;
using Web_153501_Kiselev.Domain.Entities;
using Web_153501_Kiselev.Domain.Models;
using Web_153501_Kiselev.Services.VehicleTypeService;
using Web_153501_Kiselev.Services.VehicleService;

namespace Web_153501_Kiselev.Tests
{
	public class VehicleControllerTests
	{
		private List<VehicleType> GetVehicleTypes()
		{
			return new List<VehicleType>() {
				new VehicleType() { Id = Guid.Parse("8de7288e-8b6d-4aa4-87da-6aede89ed857"), Name="Cедан", NormalizedName="sedan"},
				new VehicleType() { Id = Guid.Parse("737f5b38-079b-4680-a08b-82aa05191be5"), Name="Внедорожник", NormalizedName="suv"}
			};
		}

		private List<Vehicle> GetVehicles()
		{
			return new List<Vehicle>()
			{
				new Vehicle()
				{
					Model = "G 63 AMG",
					Description = "Хороший автомобиль",
					Price = 302000,
					Type = GetVehicleTypes().Find(item => item.NormalizedName.Equals("suv"))
				},
				new Vehicle()
				{   Model = "E 200 4M Sport",
					Description = "Хороший автомобиль",
					Price = 90000,
					Type = GetVehicleTypes().Find(item => item.NormalizedName.Equals("sedan"))
				},
			};
		}

		[Fact]
		public void Index_ReturnsViewWithListOfVehicleModel()
		{
			//Arrange
			Mock<IVehicleTypeService> types_moq = new();
			types_moq.Setup(m => m.GetVehicleTypeListAsync()).ReturnsAsync(new BaseResponse<List<VehicleType>>()
			{
				Success = true,
				Data = GetVehicleTypes()
			});

			Mock<IVehicleService> vehicles_moq = new();
			vehicles_moq.Setup(m => m.GetVehicleListAsync(null, 1)).ReturnsAsync(new BaseResponse<ListModel<Vehicle>>()
			{
				Success = true,
				Message = null,
				Data = new ListModel<Vehicle>()
				{
					Items = GetVehicles(),
					CurrentPage = 1,
					TotalPages = 1
				}
			});

			var header = new Dictionary<string, StringValues>();
			var controllerContext = new ControllerContext();
			var moqHttpContext = new Mock<HttpContext>();
			moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
			controllerContext.HttpContext = moqHttpContext.Object;

			//Act
			var controller = new VehicleController(types_moq.Object, vehicles_moq.Object) { ControllerContext = controllerContext };
			var result = controller.Index(null).Result;
			//Assert
			Assert.NotNull(result);
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.IsType<ListModel<Vehicle>>(viewResult.Model);
		}

		[Fact]
		public void Index_ReturnsError404_WhenUnsuccessfullyReceivedTypes()
		{
			//Arrange
			Mock<IVehicleTypeService> types_moq = new();
			types_moq.Setup(m => m.GetVehicleTypeListAsync()).ReturnsAsync(new BaseResponse<List<VehicleType>>()
			{
				Success = false
			});

			Mock<IVehicleService> vehicles_moq = new();
			vehicles_moq.Setup(m => m.GetVehicleListAsync(null, 1)).ReturnsAsync(new BaseResponse<ListModel<Vehicle>>()
			{
				Success = true
			});

			var header = new Dictionary<string, StringValues>();
			var controllerContext = new ControllerContext();
			var moqHttpContext = new Mock<HttpContext>();
			moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
			controllerContext.HttpContext = moqHttpContext.Object;

			//Act
			var controller = new VehicleController(types_moq.Object, vehicles_moq.Object) { ControllerContext = controllerContext };
			var result = controller.Index(null).Result;
			//Assert
			Assert.NotNull(result);
			var viewResult = Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal(StatusCodes.Status404NotFound, viewResult.StatusCode);
		}

		[Fact]
		public void Index_ReturnsError404_WhenUnsuccessfullyReceivedVehicle()
		{
			//Arrange
			Mock<IVehicleTypeService> types_moq = new();
			types_moq.Setup(m => m.GetVehicleTypeListAsync()).ReturnsAsync(new BaseResponse<List<VehicleType>>()
			{
				Success = true,
				Data = GetVehicleTypes()
			});

			Mock<IVehicleService> vehicles_moq = new();
			vehicles_moq.Setup(m => m.GetVehicleListAsync(null, 1)).ReturnsAsync(new BaseResponse<ListModel<Vehicle>>()
			{
				Success = false
			});

			var header = new Dictionary<string, StringValues>();
			var controllerContext = new ControllerContext();
			var moqHttpContext = new Mock<HttpContext>();
			moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
			controllerContext.HttpContext = moqHttpContext.Object;

			//Act
			var controller = new VehicleController(types_moq.Object, vehicles_moq.Object) { ControllerContext = controllerContext };
			var result = controller.Index(null).Result;
			//Assert
			Assert.NotNull(result);
			var viewResult = Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal(StatusCodes.Status404NotFound, viewResult.StatusCode);
		}

		[Fact]
		public void Index_ViewDataContainsTypes_WhenSuccessfullyReceivedData()
		{
			//Arrange
			Mock<IVehicleTypeService> types_moq = new();
			types_moq.Setup(m => m.GetVehicleTypeListAsync()).ReturnsAsync(new BaseResponse<List<VehicleType>>()
			{
				Success = true,
				Data = GetVehicleTypes()
			});

			Mock<IVehicleService> vehicles_moq = new();
			vehicles_moq.Setup(m => m.GetVehicleListAsync(null, 1)).ReturnsAsync(new BaseResponse<ListModel<Vehicle>>()
			{
				Success = true,
				Message = null,
				Data = new ListModel<Vehicle>()
				{
					Items = GetVehicles(),
					CurrentPage = 1,
					TotalPages = 1
				}
			});

			var header = new Dictionary<string, StringValues>();
			var controllerContext = new ControllerContext();
			var moqHttpContext = new Mock<HttpContext>();
			moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
			controllerContext.HttpContext = moqHttpContext.Object;


			//Act
			var controller = new VehicleController(types_moq.Object, vehicles_moq.Object) { ControllerContext = controllerContext };
			var result = controller.Index(null).Result;
			//Assert
			Assert.NotNull(result);
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.True(viewResult.ViewData.ContainsKey("typeList"));

			var viewDataTypes = viewResult.ViewData["typeList"] as IEnumerable<VehicleType>;

			Assert.NotNull(viewDataTypes);
			Assert.Collection(GetVehicleTypes(),
				expectedType1 => Assert.Contains(expectedType1, viewDataTypes, new VehicleTypeComparer()),
				expectedType2 => Assert.Contains(expectedType2, viewDataTypes, new VehicleTypeComparer())
			);

		}

		[Fact]
		public void Index_ViewDataContainsValidCurrentTypeValue_WhenTypeParameterIsNotNull()
		{
			//Arrange
			Mock<IVehicleTypeService> types_moq = new();
			types_moq.Setup(m => m.GetVehicleTypeListAsync()).ReturnsAsync(new BaseResponse<List<VehicleType>>()
			{
				Success = true,
				Data = GetVehicleTypes()
			});

			Mock<IVehicleService> vehicles_moq = new();
			vehicles_moq.Setup(m => m.GetVehicleListAsync("suv", 1)).ReturnsAsync(new BaseResponse<ListModel<Vehicle>>()
			{
				Success = true,
				Message = null,
				Data = new ListModel<Vehicle>()
				{
					Items = GetVehicles(),
					CurrentPage = 1,
					TotalPages = 1
				}
			});

			var header = new Dictionary<string, StringValues>();
			var controllerContext = new ControllerContext();
			var moqHttpContext = new Mock<HttpContext>();
			moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
			controllerContext.HttpContext = moqHttpContext.Object;

			//Act
			var controller = new VehicleController(types_moq.Object, vehicles_moq.Object) { ControllerContext = controllerContext };
			var result = controller.Index("suv", 1).Result;
			//Assert
			Assert.NotNull(result);
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.True(viewResult.ViewData.ContainsKey("currentType"));
			Assert.Equal("suv", viewResult.ViewData["currentType"] as string);
		}

		[Fact]
		public void Index_ViewDataContainsValidCurrentTypeValue_WhenTypeParameterIsNull()
		{
			//Arrange
			Mock<IVehicleTypeService> types_moq = new();
			types_moq.Setup(m => m.GetVehicleTypeListAsync()).ReturnsAsync(new BaseResponse<List<VehicleType>>()
			{
				Success = true,
				Data = GetVehicleTypes()
			});

			Mock<IVehicleService> vehicles_moq = new();
			vehicles_moq.Setup(m => m.GetVehicleListAsync(null, 1)).ReturnsAsync(new BaseResponse<ListModel<Vehicle>>()
			{
				Success = true,
				Message = null,
				Data = new ListModel<Vehicle>()
				{
					Items = GetVehicles(),
					CurrentPage = 1,
					TotalPages = 1
				}
			});

			var header = new Dictionary<string, StringValues>();
			var controllerContext = new ControllerContext();
			var moqHttpContext = new Mock<HttpContext>();
			moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
			controllerContext.HttpContext = moqHttpContext.Object;

			//Act
			var controller = new VehicleController(types_moq.Object, vehicles_moq.Object) { ControllerContext = controllerContext };
			var result = controller.Index(null).Result;
			//Assert
			Assert.NotNull(result);
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.True(viewResult.ViewData.ContainsKey("currentType"));
			Assert.Equal(null, viewResult.ViewData["currentType"] as string);
		}

		class VehicleTypeComparer : IEqualityComparer<VehicleType>
		{
			public bool Equals(VehicleType? x, VehicleType? y)
			{
				if (ReferenceEquals(x, y))
					return true;

				if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
					return false;

				return x.Id == y.Id && x.Name == y.Name && x.NormalizedName == y.NormalizedName;
			}

			public int GetHashCode(VehicleType obj)
			{
				int hash = 17;
				hash = hash * 23 + obj.Id.GetHashCode();
				hash = hash * 23 + obj.Name.GetHashCode();
				hash = hash * 23 + obj.NormalizedName.GetHashCode();
				return hash;
			}
		}
	}
}
