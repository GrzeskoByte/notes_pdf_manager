using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace pdf_markdown_manager.Models
{
    public class Files
    {
        [Key]
        public int id { get; set; }

        [DisplayName("Tytuł")]
        [MaxLength(50)]
        public string title { get; set; }

        [DisplayName("Zawartość")]
        [MaxLength(5000)]
        public byte[] content { get; set; }

        [DisplayName("Wielkość czcionki")]
        [MaxLength(3)]

        [ScaffoldColumn(false)]
        public string users_id { get; set; } = String.Empty;
    }
}
