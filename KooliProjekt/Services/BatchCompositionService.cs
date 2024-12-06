using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class BatchCompositionService : IBatchCompositionService
    {
        private readonly ApplicationDbContext _context;

        public BatchCompositionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BatchComposition>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BatchComposition> GetById(int id)
        {
            throw new NotImplementedException();
        }

        // Метод для получения списка с учетом параметров поиска
        public async Task<List<BatchComposition>> ListAsync(BatchCompositionSearchParameters searchParameters)
        {
            var query = _context.BatchCompositions.AsQueryable();

            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                query = query.Where(o => o.OilBatch.Contains(searchParameters.Name));
            }

            if (!string.IsNullOrEmpty(searchParameters.Description))
            {
                query = query.Where(o => o.OilBatchId.Contains(searchParameters.Description));
            }

            return await query.ToListAsync();
        }

        public Task Save(BatchComposition BatchComposition)
        {
            throw new NotImplementedException();
        }
    }
}
