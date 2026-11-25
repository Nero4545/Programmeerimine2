using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class IngredientServiceTests : ServiceTestBase
    {
        private readonly IngredientService _service;

        public IngredientServiceTests()
        {
            _service = new IngredientService(DbContext);
        }

        [Fact]
        public async Task Get_should_return_existing_item()
        {
            // Arrange
            var item = new Ingredient { Name = "Item1", Unit = "kg", UnitPrice = 2.5m, Quantity = 4, Batch = CreateBatch() };
            DbContext.Ingredients.Add(item);
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
            DbContext.Ingredients.Add(new Ingredient { Name = "Item1", Unit = "kg", UnitPrice = 2.5m, Quantity = 4, Batch = CreateBatch() });
            DbContext.Ingredients.Add(new Ingredient { Name = "Item2", Unit = "kg", UnitPrice = 2.5m, Quantity = 4, Batch = CreateBatch() });
            DbContext.Ingredients.Add(new Ingredient { Name = "Item3", Unit = "kg", UnitPrice = 2.5m, Quantity = 4, Batch = CreateBatch() });
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
            var match = new Ingredient { Name = "Cascade hops", Unit = "g", Batch = batch };
            DbContext.Ingredients.Add(match);
            DbContext.Ingredients.Add(new Ingredient { Name = "Pale malt", Unit = "kg", Batch = CreateBatch("OTHER") });
            await DbContext.SaveChangesAsync();
            var search = new IngredientSearch { Keyword = "hops", BatchId = batch.Id };

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
            var item = new Ingredient { Name = "New", Unit = "kg", UnitPrice = 2.5m, Quantity = 4, Batch = CreateBatch() };

            // Act
            await _service.Save(item);

            // Assert
            Assert.NotEqual(0, item.Id);
            Assert.Equal(1, await DbContext.Ingredients.CountAsync());
        }

        [Fact]
        public async Task Save_should_update_tracked_item()
        {
            // Arrange
            var item = new Ingredient { Name = "Old", Unit = "kg", UnitPrice = 2.5m, Quantity = 4, Batch = CreateBatch() };
            DbContext.Ingredients.Add(item);
            await DbContext.SaveChangesAsync();
            item.Name = "Changed";

            // Act
            await _service.Save(item);

            // Assert
            DbContext.ChangeTracker.Clear();
            var saved = await DbContext.Ingredients.FindAsync(item.Id);
            Assert.Equal("Changed", saved!.Name);
        }

        [Fact]
        public async Task Save_should_update_detached_item()
        {
            // Arrange
            var original = new Ingredient { Name = "Old", Unit = "kg", UnitPrice = 2.5m, Quantity = 4, Batch = CreateBatch() };
            DbContext.Ingredients.Add(original);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();
            var item = new Ingredient { Id = original.Id, BatchId = original.BatchId, Name = "Changed", Unit = "kg", UnitPrice = 1, Quantity = 1 };

            // Act
            await _service.Save(item);

            // Assert
            DbContext.ChangeTracker.Clear();
            var saved = await DbContext.Ingredients.FindAsync(original.Id);
            Assert.Equal("Changed", saved!.Name);
        }

        [Fact]
        public async Task Delete_should_remove_existing_item()
        {
            // Arrange
            var item = new Ingredient { Name = "Item1", Unit = "kg", UnitPrice = 2.5m, Quantity = 4, Batch = CreateBatch() };
            DbContext.Ingredients.Add(item);
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(item.Id);

            // Assert
            Assert.Equal(0, await DbContext.Ingredients.CountAsync());
        }

        [Fact]
        public async Task Delete_should_do_nothing_when_item_is_missing()
        {
            // Arrange
            DbContext.Ingredients.Add(new Ingredient { Name = "Item1", Unit = "kg", UnitPrice = 2.5m, Quantity = 4, Batch = CreateBatch() });
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(1000);

            // Assert
            Assert.Equal(1, await DbContext.Ingredients.CountAsync());
        }

        [Fact]
        public async Task Save_should_calculate_total()
        {
            // Arrange
            var item = new Ingredient { Name = "Malt", Unit = "kg", UnitPrice = 2.5m, Quantity = 4, Batch = CreateBatch() };

            // Act
            await _service.Save(item);

            // Assert
            Assert.Equal(10m, item.Total);
        }
    }
}
