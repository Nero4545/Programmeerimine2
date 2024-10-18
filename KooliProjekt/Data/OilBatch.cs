namespace KooliProjekt.Data
{
    public class OilBatch
    {
        public int Id { get; set; }
        public OilType OilType { get; set; }
        public DateTime BatchDate { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        // Navigational property
        public IList<BatchComposition> BatchCompositions { get; set; }

        public OilBatch()
        {
            BatchCompositions = new List<BatchComposition>();
        }
    }



}
