using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IBeerTypeService
    {
        Task<PagedResult<BeerType>> List(int page, int pageSize, BeerTypeSearch? search = null);
        Task<BeerType?> Get(int id);
        Task Save(BeerType item);
        Task Delete(int id);
        Task<IList<BeerType>> Lookup();
    }
}
