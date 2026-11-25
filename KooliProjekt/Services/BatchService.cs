using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class BatchService : IBatchService
    {
        private readonly ApplicationDbContext _context;

        public BatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Batch>> List(int page, int pageSize, BatchSearch? search = null)
        {
            var query = _context.Batches.Include(x => x.BeerType).AsQueryable();

            search = search ?? new BatchSearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(x => x.Code.Contains(search.Keyword) || (x.Description != null && x.Description.Contains(search.Keyword)));
            }

            if (search.BeerTypeId.HasValue)
            {
                query = query.Where(x => x.BeerTypeId == search.BeerTypeId.Value);
            }

            if (search.DateFrom.HasValue)
            {
                query = query.Where(x => x.BatchDate >= search.DateFrom.Value);
            }

            if (search.DateTo.HasValue)
            {
                query = query.Where(x => x.BatchDate <= search.DateTo.Value);
            }

            return await query
                .OrderByDescending(x => x.BatchDate)
                .GetPagedAsync(page, pageSize);
        }

        public async Task<Batch?> Get(int id)
        {
            return await _context.Batches
                .Include(x => x.BeerType)
                .Include(x => x.Ingredients)
                .Include(x => x.LogEntries).ThenInclude(x => x.User)
                .Include(x => x.TastingEntries).ThenInclude(x => x.User)
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Save(Batch item)
        {
            if (item.Id == 0)
            {
                _context.Batches.Add(item);
            }
            else if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.Batches.Update(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.Batches.FindAsync(id);
            if (item == null)
            {
                return;
            }

            _context.Batches.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Batch>> Lookup()
        {
            return await _context.Batches.OrderBy(x => x.Code).ToListAsync();
        }
    }
}
