using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface ITastingEntryService
    {
        Task<PagedResult<TastingEntry>> List(int page, int pageSize, TastingEntrySearch? search = null);
        Task<TastingEntry?> Get(int id);
        Task Save(TastingEntry item);
        Task Delete(int id);
    }
}
