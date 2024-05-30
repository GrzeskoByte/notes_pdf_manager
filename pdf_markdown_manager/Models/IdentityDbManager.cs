using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data.Entity;
namespace pdf_markdown_manager.Models
{
    public class IdentityDbManager : IdentityDbContext<AuthUser>
    {
        public IdentityDbManager(DbContextOptions<IdentityDbManager> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }
    }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<AuthUser>
    {
        public void Configure(EntityTypeBuilder<AuthUser> builder)
        {
            builder.Property(x => x.Email).HasMaxLength(15);
            builder.Property(x => x.Password).HasMaxLength(15);
        }
    }

}


