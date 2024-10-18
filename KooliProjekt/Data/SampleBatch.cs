namespace KooliProjekt.Data
{
    public class SampleBatch
    {
        public int Id { get; set; }
        public string Description { get; set; } // Max length: 255
        public User User { get; set; } // The user who tested the sample
    }


}