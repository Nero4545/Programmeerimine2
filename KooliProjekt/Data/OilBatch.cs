namespace KooliProjekt.Data
{
    public class OilBatch
    {
        public int Id { get; set; }
        public OilType OilType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Code { get; set; } // Max length: 15
        public string Description { get; set; } // Max length: 255
        public List<BatchComposition> BatchCompositions { get; set; }
    }



}
