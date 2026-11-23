using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class TastingEntryService : ITastingEntryService
    {
        private readonly ApplicationDbContext _context;

        public TastingEntryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TastingEntry>> List(int page, int pageSize, TastingEntrySearch? search = null)
        {
            var query = _context.TastingEntries.Include(x => x.Batch).Include(x => x.User).AsQueryable();

            search = search ?? new TastingEntrySearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(x => x.Comment != null && x.Comment.Contains(search.Keyword));
            }

            if (search.BatchId.HasValue)
            {
                query = query.Where(x => x.BatchId == search.BatchId.Value);
            }

            if (search.MinRating.HasValue)
            {
                query = query.Where(x => x.Rating >= search.MinRating.Value);
            }

            return await query
                .OrderByDescending(x => x.Date)
                .GetPagedAsync(page, pageSize);
        }

        public async Task<TastingEntry?> Get(int id)
        {
            return await _context.TastingEntries
                .Include(x => x.Batch)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Save(TastingEntry item)
        {
            if (item.Id == 0)
            {
                _context.TastingEntries.Add(item);
            }
            else
            {
                _context.TastingEntries.Update(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.TastingEntries.FindAsync(id);
            if (item == null)
            {
                return;
            }

            _context.TastingEntries.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
