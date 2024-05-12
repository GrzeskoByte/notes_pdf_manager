using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace pdf_markdown_manager.Models
{
    public class Users
    {
        [Key]
        [Required]
        public int id { get; set; }


        [DisplayName("Imię")]
        [MaxLength(15)]
        public string first_name { get; set; }

        [DisplayName("Nazwisko")]
        public string last_name { get; set; }

        [DisplayName("Data urodzenia")]
        public DateTime birth_date { get; set; }


        [ScaffoldColumn(false)]
        public int roles_id { get; set; }

        [ScaffoldColumn(false)]
        public int credentials_id { get; set; }

        [ScaffoldColumn(false)]
        public int settings_id { get; set; }
    }
}
