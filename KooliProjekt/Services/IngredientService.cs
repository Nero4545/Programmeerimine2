using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly ApplicationDbContext _context;

        public IngredientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Ingredient>> List(int page, int pageSize, IngredientSearch? search = null)
        {
            var query = _context.Ingredients.Include(x => x.Batch).AsQueryable();

            search = search ?? new IngredientSearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(x => x.Name.Contains(search.Keyword));
            }

            if (search.BatchId.HasValue)
            {
                query = query.Where(x => x.BatchId == search.BatchId.Value);
            }

            return await query
                .OrderBy(x => x.Batch!.Code).ThenBy(x => x.Name)
                .GetPagedAsync(page, pageSize);
        }

        public async Task<Ingredient?> Get(int id)
        {
            return await _context.Ingredients
                .Include(x => x.Batch)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Save(Ingredient item)
        {
            item.Total = Math.Round(item.UnitPrice * item.Quantity, 2);

            if (item.Id == 0)
            {
                _context.Ingredients.Add(item);
            }
            else if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.Ingredients.Update(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.Ingredients.FindAsync(id);
            if (item == null)
            {
                return;
            }

            _context.Ingredients.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
