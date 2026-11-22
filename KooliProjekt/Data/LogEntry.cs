using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
    // Logikanne
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
        public IdentityUser? User { get; set; }
    }
}
