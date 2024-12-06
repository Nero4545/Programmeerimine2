using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    //public class OilTypeService : IOilTypeService
    //{
    //    private readonly ApplicationDbContext _context;

    //    public OilTypeService(ApplicationDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<IEnumerable<OilType>> GetAll()
    //    {
    //        return await _context.OilTypes.ToListAsync();
    //    }

    //    public async Task<OilType> GetById(int id)
    //    {
    //        return await _context.OilTypes.FirstOrDefaultAsync(o => o.Id == id);
    //    }

    //    public async Task Save(OilType oilType)
    //    {
    //        if (oilType.Id == 0)
    //        {
    //            _context.OilTypes.Add(oilType);
    //        }
    //        else
    //        {
    //            _context.OilTypes.Update(oilType);
    //        }

    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task Delete(int id)
    //    {
    //        var oilType = await _context.OilTypes.FindAsync(id);
    //        if (oilType != null)
    //        {
    //            _context.OilTypes.Remove(oilType);
    //            await _context.SaveChangesAsync();
    //        }
    //    }
    //}

    //public class OilBatchService : IOilBatchService
    //{
    //    private readonly ApplicationDbContext _context;

    //    public OilBatchService(ApplicationDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<IEnumerable<OilBatch>> GetAll()
    //    {
    //        return await _context.OilBatches.ToListAsync();
    //    }

    //    public async Task<OilBatch> GetById(int id)
    //    {
    //        return await _context.OilBatches.FirstOrDefaultAsync(o => o.Id == id);
    //    }

    //    public async Task Save(OilBatch oilBatch)
    //    {
    //        if (oilBatch.Id == 0)
    //        {
    //            _context.OilBatch.Add(oilBatch);
    //        }
    //        else
    //        {
    //            _context.OilBatches.Update(oilBatch);
    //        }

    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task Delete(int id)
    //    {
    //        var oilBatch = await _context.OilBatches.FindAsync(id);
    //        if (oilBatch != null)
    //        {
    //            _context.OilBatches.Remove(oilBatch);
    //            await _context.SaveChangesAsync();
    //        }
    //    }
    //}
}
