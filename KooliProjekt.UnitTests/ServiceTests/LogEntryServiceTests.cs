using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class LogEntryServiceTests : ServiceTestBase
    {
        private readonly LogEntryService _service;

        public LogEntryServiceTests()
        {
            _service = new LogEntryService(DbContext);
        }

        [Fact]
        public async Task Get_should_return_existing_item()
        {
            // Arrange
            var item = new LogEntry { Description = "Item1", Date = new DateTime(2024, 1, 15), Batch = CreateBatch() };
            DbContext.LogEntries.Add(item);
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
            DbContext.LogEntries.Add(new LogEntry { Description = "Item1", Date = new DateTime(2024, 1, 15), Batch = CreateBatch() });
            DbContext.LogEntries.Add(new LogEntry { Description = "Item2", Date = new DateTime(2024, 1, 15), Batch = CreateBatch() });
            DbContext.LogEntries.Add(new LogEntry { Description = "Item3", Date = new DateTime(2024, 1, 15), Batch = CreateBatch() });
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
            var match = new LogEntry { Description = "Dry hopping", Date = new DateTime(2024, 5, 10), Batch = batch };
            DbContext.LogEntries.Add(match);
            DbContext.LogEntries.Add(new LogEntry { Description = "Mashing", Date = new DateTime(2024, 1, 1), Batch = CreateBatch("OTHER") });
            await DbContext.SaveChangesAsync();
            var search = new LogEntrySearch
            {
                Keyword = "hopping",
                BatchId = batch.Id,
                DateFrom = new DateTime(2024, 5, 1),
                DateTo = new DateTime(2024, 5, 31)
            };

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
            var item = new LogEntry { Description = "New", Date = new DateTime(2024, 1, 15), Batch = CreateBatch() };

            // Act
            await _service.Save(item);

            // Assert
            Assert.NotEqual(0, item.Id);
            Assert.Equal(1, await DbContext.LogEntries.CountAsync());
        }

        [Fact]
        public async Task Save_should_update_tracked_item()
        {
            // Arrange
            var item = new LogEntry { Description = "Old", Date = new DateTime(2024, 1, 15), Batch = CreateBatch() };
            DbContext.LogEntries.Add(item);
            await DbContext.SaveChangesAsync();
            item.Description = "Changed";

            // Act
            await _service.Save(item);

            // Assert
            DbContext.ChangeTracker.Clear();
            var saved = await DbContext.LogEntries.FindAsync(item.Id);
            Assert.Equal("Changed", saved!.Description);
        }

        [Fact]
        public async Task Save_should_update_detached_item()
        {
            // Arrange
            var original = new LogEntry { Description = "Old", Date = new DateTime(2024, 1, 15), Batch = CreateBatch() };
            DbContext.LogEntries.Add(original);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();
            var item = new LogEntry { Id = original.Id, BatchId = original.BatchId, Description = "Changed", Date = original.Date };

            // Act
            await _service.Save(item);

            // Assert
            DbContext.ChangeTracker.Clear();
            var saved = await DbContext.LogEntries.FindAsync(original.Id);
            Assert.Equal("Changed", saved!.Description);
        }

        [Fact]
        public async Task Delete_should_remove_existing_item()
        {
            // Arrange
            var item = new LogEntry { Description = "Item1", Date = new DateTime(2024, 1, 15), Batch = CreateBatch() };
            DbContext.LogEntries.Add(item);
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(item.Id);

            // Assert
            Assert.Equal(0, await DbContext.LogEntries.CountAsync());
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_item_is_missing()
        {
            // Arrange
            DbContext.LogEntries.Add(new LogEntry { Description = "Item1", Date = new DateTime(2024, 1, 15), Batch = CreateBatch() });
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(1000);

            // Assert
            Assert.Equal(1, await DbContext.LogEntries.CountAsync());
        }
    }
}
