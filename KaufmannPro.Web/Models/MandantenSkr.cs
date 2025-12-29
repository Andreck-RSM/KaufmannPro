#nullable enable

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaufmannPro.Web.Models
{
    public class MandantenSkr
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key zum Mandanten
        [Required]
        public int MandantId { get; set; }

        [ForeignKey(nameof(MandantId))]
        public Mandant Mandant { get; set; } = null!;  // EF Core initialisiert automatisch

        // Beispielspalte: SKR-Nummer
        [Required]
        [MaxLength(20)]
        public string SkrNummer { get; set; } = null!;

        // Optional: weitere Felder
        [MaxLength(100)]
        public string? Beschreibung { get; set; }
    }
}
