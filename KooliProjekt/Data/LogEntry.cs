namespace KooliProjekt.Data
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } // Max length: 255
    }


}
