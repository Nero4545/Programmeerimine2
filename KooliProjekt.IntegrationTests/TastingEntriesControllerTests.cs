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
    public class TastingEntriesControllerTests : TestBase
    {
        [Fact]
        public async Task Index_should_return_success()
        {
            // Arrange
            var batch = AddBatch();
            var item = new TastingEntry { BatchId = batch.Id, Date = new DateTime(2024, 1, 15), Rating = 7, Comment = "Good" };
            DbContext.TastingEntries.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync("/TastingEntries");

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
            var item = new TastingEntry { BatchId = batch.Id, Date = new DateTime(2024, 1, 15), Rating = 7, Comment = "Good" };
            DbContext.TastingEntries.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync($"/TastingEntries/{action}/{item.Id}");

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
            using var response = await client.GetAsync($"/TastingEntries/{action}/1000");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_return_success()
        {
            // Arrange
            using var client = CreateClient();

            // Act
            using var response = await client.GetAsync("/TastingEntries/Create");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_should_save_valid_item()
        {
            // Arrange
            var batch = AddBatch();
            EnsureTestUser();
            using var client = CreateClient();
            var countBefore = DbContext.TastingEntries.Count();
            var values = new Dictionary<string, string>
            {
                { "BatchId", batch.Id.ToString() }, { "Date", "2024-05-10" }, { "Rating", "8" }, { "Comment", "Fruity" }
            };

            // Act
            using var response = await PostFormAsync(client, "/TastingEntries/Create", "/TastingEntries/Create", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal(countBefore + 1, DbContext.TastingEntries.Count());
        }

        [Fact]
        public async Task Create_should_not_save_invalid_item()
        {
            // Arrange
            var batch = AddBatch();
            EnsureTestUser();
            using var client = CreateClient();
            var countBefore = DbContext.TastingEntries.Count();
            var values = new Dictionary<string, string>
            {
                { "BatchId", batch.Id.ToString() }, { "Date", "2024-05-10" }, { "Rating", "0" }
            };

            // Act
            using var response = await PostFormAsync(client, "/TastingEntries/Create", "/TastingEntries/Create", values);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(countBefore, DbContext.TastingEntries.Count());
        }

        [Fact]
        public async Task Edit_should_save_changes()
        {
            // Arrange
            var batch = AddBatch();
            var item = new TastingEntry { BatchId = batch.Id, Date = new DateTime(2024, 1, 15), Rating = 7, Comment = "Good" };
            DbContext.TastingEntries.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }, { "BatchId", item.BatchId.ToString() }, { "Date", "2024-05-10" }, { "Rating", "9" }, { "Comment", "Changed" }
            };

            // Act
            using var response = await PostFormAsync(client, $"/TastingEntries/Edit/{item.Id}", $"/TastingEntries/Edit/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            var saved = DbContext.TastingEntries.Find(item.Id);
            Assert.Equal("Changed", saved!.Comment);
        }

        [Fact]
        public async Task Edit_should_not_save_invalid_item()
        {
            // Arrange
            var batch = AddBatch();
            var item = new TastingEntry { BatchId = batch.Id, Date = new DateTime(2024, 1, 15), Rating = 7, Comment = "Good" };
            DbContext.TastingEntries.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }, { "BatchId", item.BatchId.ToString() }, { "Date", "2024-05-10" }, { "Rating", "9" }, { "Comment", "Changed" }
            };
            values["Rating"] = "0";

            // Act
            using var response = await PostFormAsync(client, $"/TastingEntries/Edit/{item.Id}", $"/TastingEntries/Edit/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            var saved = DbContext.TastingEntries.Find(item.Id);
            Assert.NotEqual("Changed", saved!.Comment);
        }

        [Fact]
        public async Task DeleteConfirmed_should_remove_item()
        {
            // Arrange
            var batch = AddBatch();
            var item = new TastingEntry { BatchId = batch.Id, Date = new DateTime(2024, 1, 15), Rating = 7, Comment = "Good" };
            DbContext.TastingEntries.Add(item);
            DbContext.SaveChanges();
            using var client = CreateClient();
            var values = new Dictionary<string, string>
            {
                { "Id", item.Id.ToString() }
            };

            // Act
            using var response = await PostFormAsync(client, $"/TastingEntries/Delete/{item.Id}", $"/TastingEntries/Delete/{item.Id}", values);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            DbContext.ChangeTracker.Clear();
            Assert.Null(DbContext.TastingEntries.Find(item.Id));
        }
    }
}
