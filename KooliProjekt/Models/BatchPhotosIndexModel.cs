using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class BatchPhotosIndexModel
    {
        public BatchPhotoSearch Search { get; set; } = new BatchPhotoSearch();
        public PagedResult<BatchPhoto> Data { get; set; } = new PagedResult<BatchPhoto>();
    }
}
