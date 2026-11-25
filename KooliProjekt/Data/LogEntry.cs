using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
    // Logikanne
    [ExcludeFromCodeCoverage]
    public class LogEntry
    {
        public int Id { get; set; }

        [Display(Name = "Batch")]
        public int BatchId { get; set; }
        public Batch? Batch { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        public string? UserId { get; set; }
        [JsonIgnore]
        public IdentityUser? User { get; set; }
    }
}
