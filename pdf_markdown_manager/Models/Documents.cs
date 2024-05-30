using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace pdf_markdown_manager.Models
{
    public class Documents
    {
        [Key]
        public int id { get; set; }

        [DisplayName("Tytuł")]
        [MaxLength(50)]
        public string title { get; set; }

        [DisplayName("Zawartość")]
        [MaxLength(5000)]
        public string content { get; set; }

        [DisplayName("Wielkość czcionki")]
        [MaxLength(3)]
        public string font_size { get; set; }

        [ScaffoldColumn(false)]
        public DateTime created_at { get; set; }

        [ScaffoldColumn(false)]
        public string users_id { get; set; } = String.Empty;

    }
}
