using KooliProjekt.Controllers;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class HomeControllerTests
    {
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _controller = new HomeController();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { TraceIdentifier = "trace-1" }
            };
        }

        [Fact]
        public void Index_should_return_index_view()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public void Privacy_should_return_privacy_view()
        {
            // Act
            var result = _controller.Privacy() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Privacy" ||
                        string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public void Error_should_return_error_view_with_request_id()
        {
            // Act
            var result = _controller.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Error" ||
                        string.IsNullOrEmpty(result.ViewName));
            var model = Assert.IsType<ErrorViewModel>(result.Model);
            Assert.Equal("trace-1", model.RequestId);
        }
    }
}
