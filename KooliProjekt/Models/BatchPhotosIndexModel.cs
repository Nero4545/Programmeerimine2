using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class BatchPhotosIndexModel
    {
        public BatchPhotoSearch Search { get; set; } = new BatchPhotoSearch();
        public PagedResult<BatchPhoto> Data { get; set; } = new PagedResult<BatchPhoto>();
    }
}
