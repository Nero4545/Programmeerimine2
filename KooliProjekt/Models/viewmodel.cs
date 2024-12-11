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

    public class ProductComponentsIndexModel
    {
        public ProductComponentSearchParameters SearchParameters { get; set; }
        public List<OilBatch> Items { get; set; }

        public static implicit operator ProductComponentsIndexModel(ProductComponentsIndexModel v)
        {
            throw new NotImplementedException();
        }
    }

    public class SampleBatchIndexModel
    {
        public SampleBatchSearchParameters SearchParameters { get; set; }
        public List<OilBatch> Items { get; set; }
    }

    public class ManufacturingLogIndexModel
    {
        public ManufacturingLogSearchParameters SearchParameters { get; set; }
        public List<OilBatch> Items { get; set; }
    }

    public class BatchCompositionIndexModel
    {
        public BatchCompositionSearchParameters SearchParameters { get; set; }
        public List<OilBatch> Items { get; set; }
    }
}
