using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    // Koostisosa
    [ExcludeFromCodeCoverage]
    public class Ingredient
    {
        public int Id { get; set; }

        [Display(Name = "Batch")]
        public int BatchId { get; set; }
        public Batch? Batch { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Unit { get; set; } = string.Empty;

        [Precision(18, 2)]
        [Range(0, 1000000)]
        [Display(Name = "Unit price")]
        public decimal UnitPrice { get; set; }

        [Precision(18, 3)]
        [Range(0, 1000000)]
        public decimal Quantity { get; set; }

        [Precision(18, 2)]
        public decimal Total { get; set; }
    }
}
