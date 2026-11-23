using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class TastingEntriesIndexModel
    {
        public TastingEntrySearch Search { get; set; } = new TastingEntrySearch();
        public PagedResult<TastingEntry> Data { get; set; } = new PagedResult<TastingEntry>();
    }
}
