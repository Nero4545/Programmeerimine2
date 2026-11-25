using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    // Õllesort
    [ExcludeFromCodeCoverage]
    public class BeerType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        public IList<Batch> Batches { get; set; }

        public BeerType()
        {
            Batches = new List<Batch>();
        }
    }
}
