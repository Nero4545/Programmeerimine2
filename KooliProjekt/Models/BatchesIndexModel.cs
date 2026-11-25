using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class BatchesIndexModel
    {
        public BatchSearch Search { get; set; } = new BatchSearch();
        public PagedResult<Batch> Data { get; set; } = new PagedResult<Batch>();
    }
}
