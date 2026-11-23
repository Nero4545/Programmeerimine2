using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Search
{
    public class TastingEntrySearch
    {
        public string? Keyword { get; set; }
        public int? BatchId { get; set; }
        public int? MinRating { get; set; }
    }
}
