using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class OilTypesIndexModel
    {
        public OilTypeSearchParameters SearchParameters { get; set; }
        public List<OilType> Items { get; set; }
    }

    public class OilBatchesIndexModel
    {
        public OilBatchSearchParameters SearchParameters { get; set; }
        public List<OilBatch> Items { get; set; }
    }
}
