using System.Security.Claims;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class BatchesControllerTests
    {
        private readonly Mock<IBatchService> _serviceMock;
        private readonly Mock<IBeerTypeService> _lookupServiceMock;
        private readonly BatchesController _controller;

        public BatchesControllerTests()
        {
            _serviceMock = new Mock<IBatchService>();
            _lookupServiceMock = new Mock<IBeerTypeService>();
            _lookupServiceMock
                .Setup(x => x.Lookup())
                .ReturnsAsync(new List<BeerType>());
            _controller = new BatchesController(_serviceMock.Object, _lookupServiceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user-1")
            }, "Test"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        private static Batch CreateItem()
        {
            return new Batch { Id = 1, Code = "B001", BeerTypeId = 1 };
        }

        [Fact]
        public async Task Index_should_return_view_with_data()
        {
            // Arrange
            var data = new PagedResult<Batch>();
            data.Results.Add(CreateItem());
            var model = new BatchesIndexModel();
            _serviceMock
                .Setup(x => x.List(1, 5, model.Search))
                .ReturnsAsync(data);

            // Act
            var result = await _controller.Index(1, model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");
            var resultModel = Assert.IsType<BatchesIndexModel>(result.Model);
            Assert.Same(data, resultModel.Data);
        }

        [Fact]
        public async Task Index_should_create_model_when_model_is_null()
        {
            // Arrange
            var data = new PagedResult<Batch>();
            _serviceMock
                .Setup(x => x.List(2, 5, It.IsAny<BatchSearch>()))
                .ReturnsAsync(data);

            // Act
            var result = await _controller.Index(2, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var resultModel = Assert.IsType<BatchesIndexModel>(result.Model);
            Assert.Same(data, resultModel.Data);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_item_is_missing()
        {
            // Arrange
            _serviceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_item()
        {
            // Arrange
            var item = CreateItem();
            _serviceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Details");
            Assert.Same(item, result.Model);
        }

        [Fact]
        public async Task Create_should_return_view_with_new_item()
        {
            // Act
            var result = await _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
            Assert.IsType<Batch>(result.Model);
            Assert.NotNull(result.ViewData["BeerTypeId"]);
        }

        [Fact]
        public async Task Create_should_save_and_redirect_when_model_is_valid()
        {
            // Arrange
            var item = CreateItem();
            item.Id = 0;
            _serviceMock
                .Setup(x => x.Save(item))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Create(item) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _serviceMock.VerifyAll();
        }

        [Fact]
        public async Task Create_should_return_view_when_model_is_invalid()
        {
            // Arrange
            var item = CreateItem();
            _controller.ModelState.AddModelError("key", "error");

            // Act
            var result = await _controller.Create(item) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
            Assert.Same(item, result.Model);
            _serviceMock.Verify(x => x.Save(It.IsAny<Batch>()), Times.Never);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_missing()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_item_is_missing()
        {
            // Arrange
            _serviceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_item()
        {
            // Arrange
            var item = CreateItem();
            _serviceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Edit");
            Assert.Same(item, result.Model);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_ids_differ()
        {
            // Arrange
            var item = CreateItem();

            // Act
            var result = await _controller.Edit(2, item);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_save_and_redirect_when_model_is_valid()
        {
            // Arrange
            var item = CreateItem();
            _serviceMock
                .Setup(x => x.Save(item))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Edit(1, item) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _serviceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_view_when_model_is_invalid()
        {
            // Arrange
            var item = CreateItem();
            _controller.ModelState.AddModelError("key", "error");

            // Act
            var result = await _controller.Edit(1, item) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Edit");
            Assert.Same(item, result.Model);
            _serviceMock.Verify(x => x.Save(It.IsAny<Batch>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_missing()
        {
            // Act
            var result = await _controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
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
        }

        [Fact]
        public async Task Delete_should_return_view_with_item()
        {
            // Arrange
            var item = CreateItem();
            _serviceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Delete");
            Assert.Same(item, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_and_redirect()
        {
            // Arrange
            _serviceMock
                .Setup(x => x.Delete(1))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _serviceMock.VerifyAll();
        }
    }
}
