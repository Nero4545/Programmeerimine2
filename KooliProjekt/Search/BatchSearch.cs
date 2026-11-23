using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Search
{
    public class BatchSearch
    {
        public string? Keyword { get; set; }
        public int? BeerTypeId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }
    }
}
