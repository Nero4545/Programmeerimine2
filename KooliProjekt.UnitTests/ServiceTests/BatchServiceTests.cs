using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class BatchServiceTests : ServiceTestBase
    {
        private readonly BatchService _service;

        public BatchServiceTests()
        {
            _service = new BatchService(DbContext);
        }

        [Fact]
        public async Task Get_should_return_existing_item()
        {
            // Arrange
            var item = CreateBatch("Item1");
            DbContext.Batches.Add(item);
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
            DbContext.Batches.Add(CreateBatch("Item1"));
            DbContext.Batches.Add(CreateBatch("Item2"));
            DbContext.Batches.Add(CreateBatch("Item3"));
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
            var match = CreateBatch("MATCH");
            match.BatchDate = new DateTime(2024, 5, 10);
            DbContext.Batches.Add(match);
            DbContext.Batches.Add(CreateBatch("OTHER"));
            await DbContext.SaveChangesAsync();
            var search = new BatchSearch
            {
                Keyword = "MAT",
                BeerTypeId = match.BeerTypeId,
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
            var item = CreateBatch("New");

            // Act
            await _service.Save(item);

            // Assert
            Assert.NotEqual(0, item.Id);
            Assert.Equal(1, await DbContext.Batches.CountAsync());
        }

        [Fact]
        public async Task Save_should_update_tracked_item()
        {
            // Arrange
            var item = CreateBatch("Old");
            DbContext.Batches.Add(item);
            await DbContext.SaveChangesAsync();
            item.Code = "Changed";

            // Act
            await _service.Save(item);

            // Assert
            DbContext.ChangeTracker.Clear();
            var saved = await DbContext.Batches.FindAsync(item.Id);
            Assert.Equal("Changed", saved!.Code);
        }

        [Fact]
        public async Task Save_should_update_detached_item()
        {
            // Arrange
            var original = CreateBatch("Old");
            DbContext.Batches.Add(original);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();
            var item = new Batch { Id = original.Id, BeerTypeId = original.BeerTypeId, Code = "Changed", BatchDate = original.BatchDate };

            // Act
            await _service.Save(item);

            // Assert
            DbContext.ChangeTracker.Clear();
            var saved = await DbContext.Batches.FindAsync(original.Id);
            Assert.Equal("Changed", saved!.Code);
        }

        [Fact]
        public async Task Delete_should_remove_existing_item()
        {
            // Arrange
            var item = CreateBatch("Item1");
            DbContext.Batches.Add(item);
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(item.Id);

            // Assert
            Assert.Equal(0, await DbContext.Batches.CountAsync());
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_item_is_missing()
        {
            // Arrange
            DbContext.Batches.Add(CreateBatch("Item1"));
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(1000);

            // Assert
            Assert.Equal(1, await DbContext.Batches.CountAsync());
        }

        [Fact]
        public async Task Lookup_should_return_all_items()
        {
            // Arrange
            DbContext.Batches.Add(CreateBatch("A1"));
            DbContext.Batches.Add(CreateBatch("A2"));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Lookup();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}
