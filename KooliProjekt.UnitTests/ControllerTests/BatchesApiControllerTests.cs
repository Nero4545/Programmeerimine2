using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class BatchesApiControllerTests
    {
        private readonly Mock<IBatchService> _serviceMock;
        private readonly BatchesApiController _controller;

        public BatchesApiControllerTests()
        {
            _serviceMock = new Mock<IBatchService>();
            _controller = new BatchesApiController(_serviceMock.Object);
        }

        [Fact]
        public async Task Get_should_return_all_items()
        {
            // Arrange
            var data = new PagedResult<Batch>();
            data.Results.Add(new Batch { Id = 1, Code = "B001", BeerTypeId = 1, BatchDate = new DateTime(2024, 1, 1) });
            _serviceMock
                .Setup(x => x.List(1, 10000, null))
                .ReturnsAsync(data);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task Get_by_id_should_return_item()
        {
            // Arrange
            var item = new Batch { Id = 1, Code = "B001", BeerTypeId = 1, BatchDate = new DateTime(2024, 1, 1) };
            _serviceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.Same(item, result.Value);
        }

        [Fact]
        public async Task Get_by_id_should_return_notfound_when_item_is_missing()
        {
            // Arrange
            _serviceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_should_save_new_item()
        {
            // Arrange
            var item = new Batch { Id = 1, Code = "B001", BeerTypeId = 1, BatchDate = new DateTime(2024, 1, 1) };
            _serviceMock
                .Setup(x => x.Save(item))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Post(item);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Same(item, okResult.Value);
            Assert.Equal(0, item.Id);
            _serviceMock.VerifyAll();
        }

        [Fact]
        public async Task Put_should_return_badrequest_when_ids_differ()
        {
            // Arrange
            var item = new Batch { Id = 1, Code = "B001", BeerTypeId = 1, BatchDate = new DateTime(2024, 1, 1) };

            // Act
            var result = await _controller.Put(2, item);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Put_should_return_notfound_when_item_is_missing()
        {
            // Arrange
            var item = new Batch { Id = 1, Code = "B001", BeerTypeId = 1, BatchDate = new DateTime(2024, 1, 1) };
            _serviceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _controller.Put(1, item);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Put_should_update_existing_item()
        {
            // Arrange
            var existing = new Batch { Id = 1, Code = "B001", BeerTypeId = 1, BatchDate = new DateTime(2024, 1, 1) };
            var changed = new Batch { Id = 1, Code = "B002", BeerTypeId = 2, BatchDate = new DateTime(2024, 2, 1), Description = "New", Summary = "Done" };
            _serviceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync(existing);
            _serviceMock
                .Setup(x => x.Save(existing))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Put(1, changed);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal("B002", existing.Code);
            Assert.Equal(2, existing.BeerTypeId);
            Assert.Equal(new DateTime(2024, 2, 1), existing.BatchDate);
            Assert.Equal("New", existing.Description);
            Assert.Equal("Done", existing.Summary);
            _serviceMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_item_is_missing()
        {
            // Arrange
            _serviceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _serviceMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_delete_existing_item()
        {
            // Arrange
            _serviceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync(new Batch { Id = 1, Code = "B001", BeerTypeId = 1, BatchDate = new DateTime(2024, 1, 1) });
            _serviceMock
                .Setup(x => x.Delete(1))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.VerifyAll();
        }
    }
}
