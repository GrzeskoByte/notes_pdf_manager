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
        [MinLength(5,ErrorMessage ="Pole tytułu musi mieć co najmniej 5 znaków")]
        [Required(ErrorMessage = "Pole tytułu jest wymagane.")]
        public string title { get; set; }

        [DisplayName("Zawartość")]
        [MaxLength(5000)]
        [Required(ErrorMessage = "Pole zawartości notatki jest wymagane.")]
        [MinLength(10,ErrorMessage ="Pole zawartości notatki musi mieć co najmniej 10 znaków")]
        public string content { get; set; }

        [DisplayName("Wielkość czcionki")]
        [Required(ErrorMessage = "Pole wielkości czcionki jest wymagane jest wymagane.")]
        [MaxLength(3)]
        public string font_size { get; set; } = "12";

        [DisplayName("Stworzone")]
        public DateTime created_at { get; set; }

        [ScaffoldColumn(false)]
        public string users_id { get; set; } = String.Empty;

    }
}
