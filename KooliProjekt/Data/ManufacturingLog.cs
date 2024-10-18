namespace KooliProjekt.Data
{
    public class ManufacturingLog
    {
        public int Id { get; set; }
        public int SampleBatchId { get; set; }
        public SampleBatch SampleBatch { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }


}
