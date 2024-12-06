using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OilBatchService : IOilBatchService
    {
        private readonly ApplicationDbContext _context;

        public OilBatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Метод для получения списка с учетом параметров поиска
        public async Task<List<OilBatch>> ListAsync(OilBatchSearchParameters searchParameters)
        {
            var query = _context.OilBatches.AsQueryable();

            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                query = query.Where(o => o.Code.Contains(searchParameters.Name));
            }

            if (!string.IsNullOrEmpty(searchParameters.Description))
            {
                query = query.Where(o => o.Description.Contains(searchParameters.Description));
            }

            return await query.ToListAsync();
        }
    }
}
