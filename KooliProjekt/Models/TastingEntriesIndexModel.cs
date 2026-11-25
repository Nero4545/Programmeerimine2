using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class TastingEntriesIndexModel
    {
        public TastingEntrySearch Search { get; set; } = new TastingEntrySearch();
        public PagedResult<TastingEntry> Data { get; set; } = new PagedResult<TastingEntry>();
    }
}
