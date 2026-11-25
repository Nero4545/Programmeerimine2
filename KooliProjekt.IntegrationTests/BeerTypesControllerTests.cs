using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class BeerTypesControllerTests : TestBase
    {
        [Fact]
        public async Task Index_should_return_success()
        {
            // Arrange
            var item = AddBeerType("IPA");
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync("/BeerTypes");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType!.ToString());
        }

        [Theory]
        [InlineData("Details")]
        [InlineData("Edit")]
        [InlineData("Delete")]
        public async Task Get_should_return_success_when_item_exists(string action)
        {
            // Arrange
            var item = AddBeerType("IPA");
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync($"/BeerTypes/{action}/{item.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("Details")]
        [InlineData("Edit")]
        [InlineData("Delete")]
        public async Task Get_should_return_notfound_when_item_is_missing(string action)
        {
            // Arrange
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync($"/BeerTypes/{action}/1000");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_return_success()
        {
            // Arrange
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync("/BeerTypes/Create");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_should_save_valid_item()
        {
            // Arrange
                        using var client = CreateClient();
            var countBefore = DbContext.BeerTypes.Count();
            var values = new Dictionary<string, string>
            {
                { "Name", "Stout" }, { "Description", "Dark beer" }
            };

            // Act
            using var response = await PostFormAsync(client, "/BeerTypes/Create", "/BeerTypes/Create", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal(countBefore + 1, DbContext.BeerTypes.Count());
        }

        [Fact]
        public async Task Create_should_not_save_invalid_item()
        {
            // Arrange
                        using var client = CreateClient();
            var countBefore = DbContext.BeerTypes.Count();
            var values = new Dictionary<string, string>
            {
                { "Name", "" }
            };

            // Act
            using var response = await PostFormAsync(client, "/BeerTypes/Create", "/BeerTypes/Create", values);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(countBefore, DbContext.BeerTypes.Count());
        }

        [Fact]
        public async Task Edit_should_save_changes()
        {
            // Arrange
            var item = AddBeerType("IPA");
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }, { "Name", "Changed" }
            };

            // Act
            using var response = await PostFormAsync(client, $"/BeerTypes/Edit/{item.Id}", $"/BeerTypes/Edit/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            var saved = DbContext.BeerTypes.Find(item.Id);
            Assert.Equal("Changed", saved!.Name);
        }

        [Fact]
        public async Task Edit_should_not_save_invalid_item()
        {
            // Arrange
            var item = AddBeerType("IPA");
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }, { "Name", "Changed" }
            };
            values["Name"] = "";

            // Act
            using var response = await PostFormAsync(client, $"/BeerTypes/Edit/{item.Id}", $"/BeerTypes/Edit/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            var saved = DbContext.BeerTypes.Find(item.Id);
            Assert.NotEqual("Changed", saved!.Name);
        }

        [Fact]
        public async Task DeleteConfirmed_should_remove_item()
        {
            // Arrange
            var item = AddBeerType("IPA");
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }
            };

            // Act
            using var response = await PostFormAsync(client, $"/BeerTypes/Delete/{item.Id}", $"/BeerTypes/Delete/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            Assert.Null(DbContext.BeerTypes.Find(item.Id));
        }
    }
}
