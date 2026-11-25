using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    // Partii foto
    [ExcludeFromCodeCoverage]
    public class BatchPhoto
    {
        public int Id { get; set; }

        [Display(Name = "Batch")]
        public int BatchId { get; set; }
        public Batch? Batch { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Image URL")]
        public string Url { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
