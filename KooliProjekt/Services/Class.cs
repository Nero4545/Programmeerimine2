using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public class OilTypeService : IOilTypeService
    {
        private readonly ApplicationDbContext _context;

        public OilTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OilType>> GetAll()
        {
            return await _context.OilTypes.ToListAsync();
        }

        public async Task<OilType> GetById(int id)
        {
            return await _context.OilTypes.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task Save(OilType oilType)
        {
            if (oilType.Id == 0)
            {
                _context.OilTypes.Add(oilType);
            }
            else
            {
                _context.OilTypes.Update(oilType);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var oilType = await _context.OilTypes.FindAsync(id);
            if (oilType != null)
            {
                _context.OilTypes.Remove(oilType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
