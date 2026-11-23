using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IBatchPhotoService
    {
        Task<PagedResult<BatchPhoto>> List(int page, int pageSize, BatchPhotoSearch? search = null);
        Task<BatchPhoto?> Get(int id);
        Task Save(BatchPhoto item);
        Task Delete(int id);
    }
}
