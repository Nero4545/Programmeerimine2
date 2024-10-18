namespace KooliProjekt.Data
{
    public class OilType
    {
        public int Id { get; set; }
        public string Name { get; set; } // Max length: 50
        public string Description { get; set; } // Max length: 255
        public List<SampleBatch> SampleBatches { get; set; } // A list of sample batches, optional
    }


}