using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ManufacturingLogService : IManufacturingLogService
    {
        private readonly ApplicationDbContext _context;

        public ManufacturingLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ManufacturingLog>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ManufacturingLog> GetById(int id)
        {
            throw new NotImplementedException();
        }

        // Метод для получения списка с учетом параметров поиска
        public async Task<List<ManufacturingLog>> ListAsync(ManufacturingLogSearchParameters searchParameters)
        {
            var query = _context.ManufacturingLogs.AsQueryable();

            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                query = query.Where(o => o.Comments.Contains(searchParameters.Name));
            }

            if (!string.IsNullOrEmpty(searchParameters.Description))
            {
                query = query.Where(o => o.Date.Contains(searchParameters.Description));
            }

            return await query.ToListAsync();
        }

        public Task Save(ManufacturingLog ManufacturingLog)
        {
            throw new NotImplementedException();
        }
    }
}
