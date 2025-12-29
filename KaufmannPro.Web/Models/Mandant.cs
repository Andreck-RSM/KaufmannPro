#nullable enable

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KaufmannPro.Web.Models
{
    public class Mandant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        // Navigation zu MandantenSkr (1:n)
        public ICollection<MandantenSkr> SkrZuordnungen { get; set; } = new List<MandantenSkr>();

        // Optional: weitere Felder
        [MaxLength(200)]
        public string? Adresse { get; set; }
    }
}
