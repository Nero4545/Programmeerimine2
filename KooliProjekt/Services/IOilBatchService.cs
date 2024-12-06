using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IOilBatchService
    {
        Task<IEnumerable<OilBatch>> GetAll();
        Task<OilBatch> GetById(int id);
        Task Save(OilBatch OilBatch);
        Task Delete(int id);

        Task<List<OilBatch>> ListAsync(OilBatchSearchParameters searchParameters);
    }
}