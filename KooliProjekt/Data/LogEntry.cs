namespace KooliProjekt.Data
{
    public class LogEntry
    {
        public int Id { get; set; }
        public int SampleBatchId { get; set; }
        public SampleBatch SampleBatch { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
