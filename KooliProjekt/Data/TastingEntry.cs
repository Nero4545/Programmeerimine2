using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
    // Maitsmiste logi kanne
    [ExcludeFromCodeCoverage]
    public class TastingEntry
    {
        public int Id { get; set; }

        [Display(Name = "Batch")]
        public int BatchId { get; set; }
        public Batch? Batch { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string? UserId { get; set; }
        [JsonIgnore]
        public IdentityUser? User { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }

        [StringLength(255)]
        public string? Comment { get; set; }
    }
}
