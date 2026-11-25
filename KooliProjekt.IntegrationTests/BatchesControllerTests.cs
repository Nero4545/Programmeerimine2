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
    public class BatchesControllerTests : TestBase
    {
        [Fact]
        public async Task Index_should_return_success()
        {
            // Arrange
            var item = AddBatch("B001");
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync("/Batches");

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
            var item = AddBatch("B001");
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync($"/Batches/{action}/{item.Id}");

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
            using var response = await client.GetAsync($"/Batches/{action}/1000");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_return_success()
        {
            // Arrange
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync("/Batches/Create");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_should_save_valid_item()
        {
            // Arrange
            var beerType = AddBeerType("Porter");
            using var client = CreateClient();
            var countBefore = DbContext.Batches.Count();
            var values = new Dictionary<string, string>
            {
                { "BeerTypeId", beerType.Id.ToString() }, { "BatchDate", "2024-05-10" }, { "Code", "B100" }, { "Description", "Test batch" }
            };

            // Act
            using var response = await PostFormAsync(client, "/Batches/Create", "/Batches/Create", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal(countBefore + 1, DbContext.Batches.Count());
        }

        [Fact]
        public async Task Create_should_not_save_invalid_item()
        {
            // Arrange
            var beerType = AddBeerType("Porter");
            using var client = CreateClient();
            var countBefore = DbContext.Batches.Count();
            var values = new Dictionary<string, string>
            {
                { "BeerTypeId", beerType.Id.ToString() }, { "BatchDate", "2024-05-10" }, { "Code", "" }
            };

            // Act
            using var response = await PostFormAsync(client, "/Batches/Create", "/Batches/Create", values);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(countBefore, DbContext.Batches.Count());
        }

        [Fact]
        public async Task Edit_should_save_changes()
        {
            // Arrange
            var item = AddBatch("B001");
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }, { "BeerTypeId", item.BeerTypeId.ToString() }, { "BatchDate", "2024-06-01" }, { "Code", "Changed" }
            };

            // Act
            using var response = await PostFormAsync(client, $"/Batches/Edit/{item.Id}", $"/Batches/Edit/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            var saved = DbContext.Batches.Find(item.Id);
            Assert.Equal("Changed", saved!.Code);
        }

        [Fact]
        public async Task Edit_should_not_save_invalid_item()
        {
            // Arrange
            var item = AddBatch("B001");
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }, { "BeerTypeId", item.BeerTypeId.ToString() }, { "BatchDate", "2024-06-01" }, { "Code", "Changed" }
            };
            values["Code"] = "";

            // Act
            using var response = await PostFormAsync(client, $"/Batches/Edit/{item.Id}", $"/Batches/Edit/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            var saved = DbContext.Batches.Find(item.Id);
            Assert.NotEqual("Changed", saved!.Code);
        }

        [Fact]
        public async Task DeleteConfirmed_should_remove_item()
        {
            // Arrange
            var item = AddBatch("B001");
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }
            };

            // Act
            using var response = await PostFormAsync(client, $"/Batches/Delete/{item.Id}", $"/Batches/Delete/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            Assert.Null(DbContext.Batches.Find(item.Id));
        }
    }
}
