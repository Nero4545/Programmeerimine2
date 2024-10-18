namespace KooliProjekt.Data
{
    public class OilType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigational property
        public IList<OilBatch> OilBatches { get; set; }

        public OilType()
        {
            OilBatches = new List<OilBatch>();
        }
    }


}