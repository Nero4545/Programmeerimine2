namespace KooliProjekt.Data
{


    public class BatchComposition
    {
        public int Id { get; set; }
        public int OilBatchId { get; set; }
        public OilBatch OilBatch { get; set; }

        // Navigational property
        public IList<ProductComponent> ProductComponents { get; set; }

        public BatchComposition()
        {
            ProductComponents = new List<ProductComponent>();
        }
    }


}
