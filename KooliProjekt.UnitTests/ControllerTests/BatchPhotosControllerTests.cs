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
    public class BatchPhotosControllerTests
    {
        private readonly Mock<IBatchPhotoService> _serviceMock;
        private readonly Mock<IBatchService> _lookupServiceMock;
        private readonly BatchPhotosController _controller;

        public BatchPhotosControllerTests()
        {
            _serviceMock = new Mock<IBatchPhotoService>();
            _lookupServiceMock = new Mock<IBatchService>();
            _lookupServiceMock
                .Setup(x => x.Lookup())
                .ReturnsAsync(new List<Batch>());
            _controller = new BatchPhotosController(_serviceMock.Object, _lookupServiceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user-1")
            }, "Test"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        private static BatchPhoto CreateItem()
        {
            return new BatchPhoto { Id = 1, BatchId = 1, Url = "https://example.com/1.jpg" };
        }

        [Fact]
        public async Task Index_should_return_view_with_data()
        {
            // Arrange
            var data = new PagedResult<BatchPhoto>();
            data.Results.Add(CreateItem());
            var model = new BatchPhotosIndexModel();
            _serviceMock
                .Setup(x => x.List(1, 5, model.Search))
                .ReturnsAsync(data);

            // Act
            var result = await _controller.Index(1, model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");
            var resultModel = Assert.IsType<BatchPhotosIndexModel>(result.Model);
            Assert.Same(data, resultModel.Data);
        }

        [Fact]
        public async Task Index_should_create_model_when_model_is_null()
        {
            // Arrange
            var data = new PagedResult<BatchPhoto>();
            _serviceMock
                .Setup(x => x.List(2, 5, It.IsAny<BatchPhotoSearch>()))
                .ReturnsAsync(data);

            // Act
            var result = await _controller.Index(2, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var resultModel = Assert.IsType<BatchPhotosIndexModel>(result.Model);
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
                .ReturnsAsync((BatchPhoto?)null);

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
            var result = await _controller.Create((int?)null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
            var model = Assert.IsType<BatchPhoto>(result.Model);
            Assert.Equal(0, model.BatchId);
            Assert.NotNull(result.ViewData["BatchId"]);
        }

        [Fact]
        public async Task Create_should_preselect_batch_when_batch_id_is_given()
        {
            // Act
            var result = await _controller.Create(5) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<BatchPhoto>(result.Model);
            Assert.Equal(5, model.BatchId);
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
            _serviceMock.Verify(x => x.Save(It.IsAny<BatchPhoto>()), Times.Never);
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
                .ReturnsAsync((BatchPhoto?)null);

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
            _serviceMock.Verify(x => x.Save(It.IsAny<BatchPhoto>()), Times.Never);
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
                .ReturnsAsync((BatchPhoto?)null);

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
