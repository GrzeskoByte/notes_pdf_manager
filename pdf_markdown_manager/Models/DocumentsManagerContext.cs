using Microsoft.EntityFrameworkCore;
using pdf_markdown_manager.Models;

namespace pdf_markdown_manager.Models
{
    public class DocumentsManagerContext : DbContext
    {
        public DocumentsManagerContext(DbContextOptions<DocumentsManagerContext> options) : base(options) { }

        public DbSet<Documents> Documents { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<pdf_markdown_manager.Models.Users> Users { get; set; } = default!;

    }
}
