using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Search
{
    public class LogEntrySearch
    {
        public string? Keyword { get; set; }
        public int? BatchId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }
    }
}
