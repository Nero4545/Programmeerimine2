using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class BatchPhotoServiceTests : ServiceTestBase
    {
        private readonly BatchPhotoService _service;

        public BatchPhotoServiceTests()
        {
            _service = new BatchPhotoService(DbContext);
        }

        [Fact]
        public async Task Get_should_return_existing_item()
        {
            // Arrange
            var item = new BatchPhoto { Url = "https://example.com/Item1.jpg", Description = "Item1", Batch = CreateBatch() };
            DbContext.BatchPhotos.Add(item);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Get(item.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(item.Id, result.Id);
        }

        [Fact]
        public async Task Get_should_return_null_when_item_is_missing()
        {
            // Act
            var result = await _service.Get(1000);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task List_should_return_paged_data()
        {
            // Arrange
            DbContext.BatchPhotos.Add(new BatchPhoto { Url = "https://example.com/Item1.jpg", Description = "Item1", Batch = CreateBatch() });
            DbContext.BatchPhotos.Add(new BatchPhoto { Url = "https://example.com/Item2.jpg", Description = "Item2", Batch = CreateBatch() });
            DbContext.BatchPhotos.Add(new BatchPhoto { Url = "https://example.com/Item3.jpg", Description = "Item3", Batch = CreateBatch() });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.List(1, 2);

            // Assert
            Assert.Equal(3, result.RowCount);
            Assert.Equal(2, result.PageCount);
            Assert.Equal(2, result.Results.Count);
        }

        [Fact]
        public async Task List_should_filter_by_search_parameters()
        {
            // Arrange
            var batch = CreateBatch("MATCH");
            var match = new BatchPhoto { Url = "https://example.com/1.jpg", Description = "Foam close-up", Batch = batch };
            DbContext.BatchPhotos.Add(match);
            DbContext.BatchPhotos.Add(new BatchPhoto { Url = "https://example.com/2.jpg", Description = "Bottles", Batch = CreateBatch("OTHER") });
            await DbContext.SaveChangesAsync();
            var search = new BatchPhotoSearch { Keyword = "Foam", BatchId = batch.Id };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Single(result.Results);
            Assert.Equal(match.Id, result.Results[0].Id);
        }

        [Fact]
        public async Task Save_should_add_new_item()
        {
            // Arrange
            var item = new BatchPhoto { Url = "https://example.com/New.jpg", Description = "New", Batch = CreateBatch() };

            // Act
            await _service.Save(item);

            // Assert
            Assert.NotEqual(0, item.Id);
            Assert.Equal(1, await DbContext.BatchPhotos.CountAsync());
        }

        [Fact]
        public async Task Save_should_update_tracked_item()
        {
            // Arrange
            var item = new BatchPhoto { Url = "https://example.com/Old.jpg", Description = "Old", Batch = CreateBatch() };
            DbContext.BatchPhotos.Add(item);
            await DbContext.SaveChangesAsync();
            item.Description = "Changed";

            // Act
            await _service.Save(item);

            // Assert
            DbContext.ChangeTracker.Clear();
            var saved = await DbContext.BatchPhotos.FindAsync(item.Id);
            Assert.Equal("Changed", saved!.Description);
        }

        [Fact]
        public async Task Save_should_update_detached_item()
        {
            // Arrange
            var original = new BatchPhoto { Url = "https://example.com/Old.jpg", Description = "Old", Batch = CreateBatch() };
            DbContext.BatchPhotos.Add(original);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();
            var item = new BatchPhoto { Id = original.Id, BatchId = original.BatchId, Url = original.Url, Description = "Changed" };

            // Act
            await _service.Save(item);

            // Assert
            DbContext.ChangeTracker.Clear();
            var saved = await DbContext.BatchPhotos.FindAsync(original.Id);
            Assert.Equal("Changed", saved!.Description);
        }

        [Fact]
        public async Task Delete_should_remove_existing_item()
        {
            // Arrange
            var item = new BatchPhoto { Url = "https://example.com/Item1.jpg", Description = "Item1", Batch = CreateBatch() };
            DbContext.BatchPhotos.Add(item);
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(item.Id);

            // Assert
            Assert.Equal(0, await DbContext.BatchPhotos.CountAsync());
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_item_is_missing()
        {
            // Arrange
            DbContext.BatchPhotos.Add(new BatchPhoto { Url = "https://example.com/Item1.jpg", Description = "Item1", Batch = CreateBatch() });
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(1000);

            // Assert
            Assert.Equal(1, await DbContext.BatchPhotos.CountAsync());
        }
    }
}
