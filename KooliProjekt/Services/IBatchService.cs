using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IBatchService
    {
        Task<PagedResult<Batch>> List(int page, int pageSize, BatchSearch? search = null);
        Task<Batch?> Get(int id);
        Task Save(Batch item);
        Task Delete(int id);
        Task<IList<Batch>> Lookup();
    }
}
