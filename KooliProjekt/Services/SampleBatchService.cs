using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class SampleBatchService : ISampleBatchService
    {
        private readonly ApplicationDbContext _context;

        public SampleBatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SampleBatch>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SampleBatch> GetById(int id)
        {
            throw new NotImplementedException();
        }

        // Метод для получения списка с учетом параметров поиска
        public async Task<List<SampleBatch>> ListAsync(SampleBatchSearchParameters searchParameters)
        {
            var query = _context.SampleBatches.AsQueryable();

            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                query = query.Where(o => o.Id.Contains(searchParameters.Name));
            }

            if (!string.IsNullOrEmpty(searchParameters.Description))
            {
                query = query.Where(o => o.Description.Contains(searchParameters.Description));
            }

            return await query.ToListAsync();
        }

        public Task Save(SampleBatch SampleBatch)
        {
            throw new NotImplementedException();
        }
    }
}
