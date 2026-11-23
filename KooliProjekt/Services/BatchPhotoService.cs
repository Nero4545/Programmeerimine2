using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class BatchPhotoService : IBatchPhotoService
    {
        private readonly ApplicationDbContext _context;

        public BatchPhotoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<BatchPhoto>> List(int page, int pageSize, BatchPhotoSearch? search = null)
        {
            var query = _context.BatchPhotos.Include(x => x.Batch).AsQueryable();

            search = search ?? new BatchPhotoSearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(x => x.Description != null && x.Description.Contains(search.Keyword));
            }

            if (search.BatchId.HasValue)
            {
                query = query.Where(x => x.BatchId == search.BatchId.Value);
            }

            return await query
                .OrderBy(x => x.Batch!.Code)
                .GetPagedAsync(page, pageSize);
        }

        public async Task<BatchPhoto?> Get(int id)
        {
            return await _context.BatchPhotos
                .Include(x => x.Batch)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Save(BatchPhoto item)
        {
            if (item.Id == 0)
            {
                _context.BatchPhotos.Add(item);
            }
            else
            {
                _context.BatchPhotos.Update(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.BatchPhotos.FindAsync(id);
            if (item == null)
            {
                return;
            }

            _context.BatchPhotos.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
