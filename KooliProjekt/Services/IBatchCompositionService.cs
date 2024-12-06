using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IBatchCompositionService
    {
        Task<IEnumerable<BatchComposition>> GetAll();
        Task<BatchComposition> GetById(int id);
        Task Save(BatchComposition BatchComposition);
        Task Delete(int id);

        Task<List<BatchComposition>> ListAsync(BatchCompositionSearchParameters searchParameters);
    }
}