using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ProductComponentService : IProductComponentService
    {
        private readonly ApplicationDbContext _context;

        public ProductComponentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductComponent>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ProductComponent> GetById(int id)
        {
            throw new NotImplementedException();
        }

        // Метод для получения списка с учетом параметров поиска
        public async Task<List<ProductComponent>> ListAsync(ProductComponentSearchParameters searchParameters)
        {
            var query = _context.ProductComponents.AsQueryable();

            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                query = query.Where(o => o.Name.Contains(searchParameters.Name));
            }

            if (!string.IsNullOrEmpty(searchParameters.Description))
            {
                query = query.Where(o => o.Unit.Contains(searchParameters.Description));
            }

            return await query.ToListAsync();
        }

        public Task Save(ProductComponent ProductComponent)
        {
            throw new NotImplementedException();
        }
    }
}
