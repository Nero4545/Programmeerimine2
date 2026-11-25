using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class TastingEntryServiceTests : ServiceTestBase
    {
        private readonly TastingEntryService _service;

        public TastingEntryServiceTests()
        {
            _service = new TastingEntryService(DbContext);
        }

        [Fact]
        public async Task Get_should_return_existing_item()
        {
            // Arrange
            var item = new TastingEntry { Comment = "Item1", Rating = 7, Date = new DateTime(2024, 1, 15), Batch = CreateBatch() };
            DbContext.TastingEntries.Add(item);
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
            DbContext.TastingEntries.Add(new TastingEntry { Comment = "Item1", Rating = 7, Date = new DateTime(2024, 1, 15), Batch = CreateBatch() });
            DbContext.TastingEntries.Add(new TastingEntry { Comment = "Item2", Rating = 7, Date = new DateTime(2024, 1, 15), Batch = CreateBatch() });
            DbContext.TastingEntries.Add(new TastingEntry { Comment = "Item3", Rating = 7, Date = new DateTime(2024, 1, 15), Batch = CreateBatch() });
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
            var match = new TastingEntry { Comment = "Very fruity", Rating = 9, Batch = batch };
            DbContext.TastingEntries.Add(match);
            DbContext.TastingEntries.Add(new TastingEntry { Comment = "Too bitter", Rating = 3, Batch = CreateBatch("OTHER") });
            await DbContext.SaveChangesAsync();
            var search = new TastingEntrySearch { Keyword = "fruity", BatchId = batch.Id, MinRating = 8 };

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
            var item = new TastingEntry { Comment = "New", Rating = 7, Date = new DateTime(2024, 1, 15), Batch = CreateBatch() };

            // Act
            await _service.Save(item);

            // Assert
            Assert.NotEqual(0, item.Id);
            Assert.Equal(1, await DbContext.TastingEntries.CountAsync());
        }

        [Fact]
        public async Task Save_should_update_tracked_item()
        {
            // Arrange
            var item = new TastingEntry { Comment = "Old", Rating = 7, Date = new DateTime(2024, 1, 15), Batch = CreateBatch() };
            DbContext.TastingEntries.Add(item);
            await DbContext.SaveChangesAsync();
            item.Comment = "Changed";

            // Act
            await _service.Save(item);

            // Assert
            DbContext.ChangeTracker.Clear();
            var saved = await DbContext.TastingEntries.FindAsync(item.Id);
            Assert.Equal("Changed", saved!.Comment);
        }

        [Fact]
        public async Task Save_should_update_detached_item()
        {
            // Arrange
            var original = new TastingEntry { Comment = "Old", Rating = 7, Date = new DateTime(2024, 1, 15), Batch = CreateBatch() };
            DbContext.TastingEntries.Add(original);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();
            var item = new TastingEntry { Id = original.Id, BatchId = original.BatchId, Comment = "Changed", Rating = 5, Date = original.Date };

            // Act
            await _service.Save(item);

            // Assert
            DbContext.ChangeTracker.Clear();
            var saved = await DbContext.TastingEntries.FindAsync(original.Id);
            Assert.Equal("Changed", saved!.Comment);
        }

        [Fact]
        public async Task Delete_should_remove_existing_item()
        {
            // Arrange
            var item = new TastingEntry { Comment = "Item1", Rating = 7, Date = new DateTime(2024, 1, 15), Batch = CreateBatch() };
            DbContext.TastingEntries.Add(item);
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(item.Id);

            // Assert
            Assert.Equal(0, await DbContext.TastingEntries.CountAsync());
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_item_is_missing()
        {
            // Arrange
            DbContext.TastingEntries.Add(new TastingEntry { Comment = "Item1", Rating = 7, Date = new DateTime(2024, 1, 15), Batch = CreateBatch() });
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(1000);

            // Assert
            Assert.Equal(1, await DbContext.TastingEntries.CountAsync());
        }
    }
}
