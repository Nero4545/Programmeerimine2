using KooliProjekt.WpfApp.Api;
using Xunit;

namespace KooliProjekt.WpfApp.UnitTests
{
    public class ApiModelTests
    {
        [Fact]
        public void Result_should_have_error_when_error_is_set()
        {
            // Arrange
            var result = new Result { Error = "Error" };

            // Assert
            Assert.True(result.HasError);
        }

        [Fact]
        public void Result_should_not_have_error_by_default()
        {
            // Arrange
            var result = new Result<int> { Value = 5 };

            // Assert
            Assert.False(result.HasError);
            Assert.Equal(5, result.Value);
        }

        [Fact]
        public void BeerType_should_raise_property_changed()
        {
            // Arrange
            var item = new BeerType();
            var changed = new List<string?>();
            item.PropertyChanged += (sender, e) => changed.Add(e.PropertyName);

            // Act
            item.Id = 1;
            item.Name = "IPA";
            item.Description = "Hoppy";

            // Assert
            Assert.Equal(new[] { "Id", "Name", "Description" }, changed);
            Assert.Equal(1, item.Id);
            Assert.Equal("IPA", item.Name);
            Assert.Equal("Hoppy", item.Description);
        }
    }
}
