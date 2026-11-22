using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    // Proovipartii
    public class Batch
    {
        public int Id { get; set; }

        [Display(Name = "Beer type")]
        public int BeerTypeId { get; set; }
        public BeerType? BeerType { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Batch date")]
        public DateTime BatchDate { get; set; }

        [Required]
        [StringLength(15)]
        public string Code { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        // Kokkuvõte, kui partii on valmis ja proovitud
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        public string? Summary { get; set; }

        public IList<Ingredient> Ingredients { get; set; }
        public IList<LogEntry> LogEntries { get; set; }
        public IList<TastingEntry> TastingEntries { get; set; }
        public IList<BatchPhoto> Photos { get; set; }

        public Batch()
        {
            Ingredients = new List<Ingredient>();
            LogEntries = new List<LogEntry>();
            TastingEntries = new List<TastingEntry>();
            Photos = new List<BatchPhoto>();
        }
    }
}
