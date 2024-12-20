﻿using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OilTypeService : IOilTypeService
    {
        private readonly ApplicationDbContext _context;

        public OilTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OilType>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<OilType> GetById(int id)
        {
            throw new NotImplementedException();
        }

        // Метод для получения списка с учетом параметров поиска
        public async Task<List<OilType>> ListAsync(OilTypeSearchParameters searchParameters)
        {
            var query = _context.OilTypes.AsQueryable();

            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                query = query.Where(o => o.Name.Contains(searchParameters.Name));
            }

            if (!string.IsNullOrEmpty(searchParameters.Description))
            {
                query = query.Where(o => o.Description.Contains(searchParameters.Description));
            }

            return await query.ToListAsync();
        }

        public Task Save(OilType oilType)
        {
            throw new NotImplementedException();
        }
    }
}
