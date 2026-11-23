using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Search
{
    public class BatchPhotoSearch
    {
        public string? Keyword { get; set; }
        public int? BatchId { get; set; }
    }
}
