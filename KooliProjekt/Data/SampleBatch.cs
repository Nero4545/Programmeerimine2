namespace KooliProjekt.Data
{
    public class SampleBatch
    {
        public int Id { get; set; }
        public int OilBatchId { get; set; }
        public OilBatch OilBatch { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        // Navigational properties
        public IList<LogEntry> LogEntries { get; set; }
        public IList<ManufacturingLog> ManufacturingLogs { get; set; }

        public SampleBatch()
        {
            LogEntries = new List<LogEntry>();
            ManufacturingLogs = new List<ManufacturingLog>();
        }
    }


}