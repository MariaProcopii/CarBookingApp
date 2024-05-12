using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Vehicles.Queries;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Vehicles.Queries;

    public class GetAllModelsForVendorQueryTests
    {
        private readonly Mock<IVehicleRepository> _mockRepository;
        private readonly GetAllModelsForVendorQueryHandler _handler;

        public GetAllModelsForVendorQueryTests()
        {
            _mockRepository = new Mock<IVehicleRepository>();
            _handler = new GetAllModelsForVendorQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllModelsForVendor_WhenVendorExists_ShouldReturnListOfModels()
        {
            var vendor = "Toyota";
            var command = new GetAllModelsForVendorQuery { Vendor = vendor };
            var expectedModels = new List<string> { "Corolla", "Camry", "Rav4" };
            _mockRepository.Setup(repo => repo.GetModelsForVendorListAsynk(vendor))
                           .ReturnsAsync(expectedModels);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            _mockRepository.Verify(repo => repo.GetModelsForVendorListAsynk(vendor), Times.Once);
        }

        [Fact]
        public async Task GetAllModelsForVendor_WhenVendorDoesNotExists_ShouldReturnEmptyList()
        {
            var vendor = "NonExistingVendor";
            var expectedModels = new List<string>();
            var command = new GetAllModelsForVendorQuery { Vendor = vendor };
            _mockRepository.Setup(repo => repo.GetModelsForVendorListAsynk(vendor))
                           .ReturnsAsync(expectedModels);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(repo => repo.GetModelsForVendorListAsynk(vendor), Times.Once);
        }
    }