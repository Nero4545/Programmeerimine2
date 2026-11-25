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
    public class BatchPhotosControllerTests : TestBase
    {
        [Fact]
        public async Task Index_should_return_success()
        {
            // Arrange
            var batch = AddBatch();
            var item = new BatchPhoto { BatchId = batch.Id, Url = "https://example.com/1.jpg", Description = "Foam" };
            DbContext.BatchPhotos.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync("/BatchPhotos");

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
            var item = new BatchPhoto { BatchId = batch.Id, Url = "https://example.com/1.jpg", Description = "Foam" };
            DbContext.BatchPhotos.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync($"/BatchPhotos/{action}/{item.Id}");

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
            using var response = await client.GetAsync($"/BatchPhotos/{action}/1000");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_return_success()
        {
            // Arrange
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync("/BatchPhotos/Create");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_should_save_valid_item()
        {
            // Arrange
            var batch = AddBatch();
            using var client = CreateClient();
            var countBefore = DbContext.BatchPhotos.Count();
            var values = new Dictionary<string, string>
            {
                { "BatchId", batch.Id.ToString() }, { "Url", "https://example.com/2.jpg" }, { "Description", "Bottles" }
            };

            // Act
            using var response = await PostFormAsync(client, "/BatchPhotos/Create", "/BatchPhotos/Create", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal(countBefore + 1, DbContext.BatchPhotos.Count());
        }

        [Fact]
        public async Task Create_should_not_save_invalid_item()
        {
            // Arrange
            var batch = AddBatch();
            using var client = CreateClient();
            var countBefore = DbContext.BatchPhotos.Count();
            var values = new Dictionary<string, string>
            {
                { "BatchId", batch.Id.ToString() }, { "Url", "" }
            };

            // Act
            using var response = await PostFormAsync(client, "/BatchPhotos/Create", "/BatchPhotos/Create", values);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(countBefore, DbContext.BatchPhotos.Count());
        }

        [Fact]
        public async Task Edit_should_save_changes()
        {
            // Arrange
            var batch = AddBatch();
            var item = new BatchPhoto { BatchId = batch.Id, Url = "https://example.com/1.jpg", Description = "Foam" };
            DbContext.BatchPhotos.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }, { "BatchId", item.BatchId.ToString() }, { "Url", "https://example.com/3.jpg" }, { "Description", "Changed" }
            };

            // Act
            using var response = await PostFormAsync(client, $"/BatchPhotos/Edit/{item.Id}", $"/BatchPhotos/Edit/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            var saved = DbContext.BatchPhotos.Find(item.Id);
            Assert.Equal("Changed", saved!.Description);
        }

        [Fact]
        public async Task Edit_should_not_save_invalid_item()
        {
            // Arrange
            var batch = AddBatch();
            var item = new BatchPhoto { BatchId = batch.Id, Url = "https://example.com/1.jpg", Description = "Foam" };
            DbContext.BatchPhotos.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }, { "BatchId", item.BatchId.ToString() }, { "Url", "https://example.com/3.jpg" }, { "Description", "Changed" }
            };
            values["Url"] = "";

            // Act
            using var response = await PostFormAsync(client, $"/BatchPhotos/Edit/{item.Id}", $"/BatchPhotos/Edit/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            var saved = DbContext.BatchPhotos.Find(item.Id);
            Assert.NotEqual("Changed", saved!.Description);
        }

        [Fact]
        public async Task DeleteConfirmed_should_remove_item()
        {
            // Arrange
            var batch = AddBatch();
            var item = new BatchPhoto { BatchId = batch.Id, Url = "https://example.com/1.jpg", Description = "Foam" };
            DbContext.BatchPhotos.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }
            };

            // Act
            using var response = await PostFormAsync(client, $"/BatchPhotos/Delete/{item.Id}", $"/BatchPhotos/Delete/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            Assert.Null(DbContext.BatchPhotos.Find(item.Id));
        }
    }
}
