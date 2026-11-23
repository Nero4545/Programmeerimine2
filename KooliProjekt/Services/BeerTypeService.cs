using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class BeerTypeService : IBeerTypeService
    {
        private readonly ApplicationDbContext _context;

        public BeerTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<BeerType>> List(int page, int pageSize, BeerTypeSearch? search = null)
        {
            var query = _context.BeerTypes.AsQueryable();

            search = search ?? new BeerTypeSearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(x => x.Name.Contains(search.Keyword) || (x.Description != null && x.Description.Contains(search.Keyword)));
            }

            return await query
                .OrderBy(x => x.Name)
                .GetPagedAsync(page, pageSize);
        }

        public async Task<BeerType?> Get(int id)
        {
            return await _context.BeerTypes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Save(BeerType item)
        {
            if (item.Id == 0)
            {
                _context.BeerTypes.Add(item);
            }
            else
            {
                _context.BeerTypes.Update(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.BeerTypes.FindAsync(id);
            if (item == null)
            {
                return;
            }

            _context.BeerTypes.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<BeerType>> Lookup()
        {
            return await _context.BeerTypes.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
