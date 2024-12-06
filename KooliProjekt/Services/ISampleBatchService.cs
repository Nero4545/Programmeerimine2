using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface ISampleBatchService
    {
        Task<IEnumerable<SampleBatch>> GetAll();
        Task<SampleBatch> GetById(int id);
        Task Save(SampleBatch SampleBatch);
        Task Delete(int id);

        Task<List<SampleBatch>> ListAsync(SampleBatchSearchParameters searchParameters);
    }
}