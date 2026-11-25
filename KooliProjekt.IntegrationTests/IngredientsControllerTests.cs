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
    public class IngredientsControllerTests : TestBase
    {
        [Fact]
        public async Task Index_should_return_success()
        {
            // Arrange
            var batch = AddBatch();
            var item = new Ingredient { BatchId = batch.Id, Name = "Malt", Unit = "kg", UnitPrice = 2, Quantity = 3, Total = 6 };
            DbContext.Ingredients.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync("/Ingredients");

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
            var batch = AddBatch();
            var item = new Ingredient { BatchId = batch.Id, Name = "Malt", Unit = "kg", UnitPrice = 2, Quantity = 3, Total = 6 };
            DbContext.Ingredients.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync($"/Ingredients/{action}/{item.Id}");

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
            using var response = await client.GetAsync($"/Ingredients/{action}/1000");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_return_success()
        {
            // Arrange
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync("/Ingredients/Create");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_should_save_valid_item()
        {
            // Arrange
            var batch = AddBatch();
            using var client = CreateClient();
            var countBefore = DbContext.Ingredients.Count();
            var values = new Dictionary<string, string>
            {
                { "BatchId", batch.Id.ToString() }, { "Name", "Hops" }, { "Unit", "g" }, { "UnitPrice", "2.5" }, { "Quantity", "4" }
            };

            // Act
            using var response = await PostFormAsync(client, "/Ingredients/Create", "/Ingredients/Create", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal(countBefore + 1, DbContext.Ingredients.Count());
        }

        [Fact]
        public async Task Create_should_not_save_invalid_item()
        {
            // Arrange
            var batch = AddBatch();
            using var client = CreateClient();
            var countBefore = DbContext.Ingredients.Count();
            var values = new Dictionary<string, string>
            {
                { "BatchId", batch.Id.ToString() }, { "Name", "" }, { "Unit", "g" }, { "UnitPrice", "2.5" }, { "Quantity", "4" }
            };

            // Act
            using var response = await PostFormAsync(client, "/Ingredients/Create", "/Ingredients/Create", values);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(countBefore, DbContext.Ingredients.Count());
        }

        [Fact]
        public async Task Edit_should_save_changes()
        {
            // Arrange
            var batch = AddBatch();
            var item = new Ingredient { BatchId = batch.Id, Name = "Malt", Unit = "kg", UnitPrice = 2, Quantity = 3, Total = 6 };
            DbContext.Ingredients.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }, { "BatchId", item.BatchId.ToString() }, { "Name", "Changed" }, { "Unit", "kg" }, { "UnitPrice", "1" }, { "Quantity", "1" }
            };

            // Act
            using var response = await PostFormAsync(client, $"/Ingredients/Edit/{item.Id}", $"/Ingredients/Edit/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            var saved = DbContext.Ingredients.Find(item.Id);
            Assert.Equal("Changed", saved!.Name);
        }

        [Fact]
        public async Task Edit_should_not_save_invalid_item()
        {
            // Arrange
            var batch = AddBatch();
            var item = new Ingredient { BatchId = batch.Id, Name = "Malt", Unit = "kg", UnitPrice = 2, Quantity = 3, Total = 6 };
            DbContext.Ingredients.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }, { "BatchId", item.BatchId.ToString() }, { "Name", "Changed" }, { "Unit", "kg" }, { "UnitPrice", "1" }, { "Quantity", "1" }
            };
            values["Name"] = "";

            // Act
            using var response = await PostFormAsync(client, $"/Ingredients/Edit/{item.Id}", $"/Ingredients/Edit/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            var saved = DbContext.Ingredients.Find(item.Id);
            Assert.NotEqual("Changed", saved!.Name);
        }

        [Fact]
        public async Task DeleteConfirmed_should_remove_item()
        {
            // Arrange
            var batch = AddBatch();
            var item = new Ingredient { BatchId = batch.Id, Name = "Malt", Unit = "kg", UnitPrice = 2, Quantity = 3, Total = 6 };
            DbContext.Ingredients.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }
            };

            // Act
            using var response = await PostFormAsync(client, $"/Ingredients/Delete/{item.Id}", $"/Ingredients/Delete/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            Assert.Null(DbContext.Ingredients.Find(item.Id));
        }
    }
}
