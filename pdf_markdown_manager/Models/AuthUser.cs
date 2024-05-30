using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pdf_markdown_manager.Models
{
    [Table("AspNetUsers")]
    public class AuthUser : IdentityUser
    {

       
        [Required]
        [StringLength(15)]
        public string? Password { get; set; } = String.Empty;
    }
}
