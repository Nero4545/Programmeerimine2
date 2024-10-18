namespace KooliProjekt.Data
{


    public class BatchComposition
    {
        public int Id { get; set; }
        public SampleBatch SampleBatch { get; set; }
        public DateTime TestDate { get; set; }
        public User Tester { get; set; }
        public string Notes { get; set; } // Max length: 255
    }


}
