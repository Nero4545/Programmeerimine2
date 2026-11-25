using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class LogEntryService : ILogEntryService
    {
        private readonly ApplicationDbContext _context;

        public LogEntryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<LogEntry>> List(int page, int pageSize, LogEntrySearch? search = null)
        {
            var query = _context.LogEntries.Include(x => x.Batch).Include(x => x.User).AsQueryable();

            search = search ?? new LogEntrySearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(x => x.Description.Contains(search.Keyword));
            }

            if (search.BatchId.HasValue)
            {
                query = query.Where(x => x.BatchId == search.BatchId.Value);
            }

            if (search.DateFrom.HasValue)
            {
                query = query.Where(x => x.Date >= search.DateFrom.Value);
            }

            if (search.DateTo.HasValue)
            {
                query = query.Where(x => x.Date <= search.DateTo.Value);
            }

            return await query
                .OrderByDescending(x => x.Date)
                .GetPagedAsync(page, pageSize);
        }

        public async Task<LogEntry?> Get(int id)
        {
            return await _context.LogEntries
                .Include(x => x.Batch)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Save(LogEntry item)
        {
            if (item.Id == 0)
            {
                _context.LogEntries.Add(item);
            }
            else if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.LogEntries.Update(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.LogEntries.FindAsync(id);
            if (item == null)
            {
                return;
            }

            _context.LogEntries.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
