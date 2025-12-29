using KaufmannPro.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KaufmannPro.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Mandant> Mandanten { get; set; } = null!;
        public DbSet<MandantenSkr> MandantenSkr { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser<int>>().ToTable("benutzer");
            builder.Entity<IdentityRole<int>>().ToTable("rollen");
            builder.Entity<IdentityUserRole<int>>().ToTable("benutzerrollen");
            builder.Entity<IdentityUserClaim<int>>().ToTable("benutzerclaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("benutzerlogins");
            builder.Entity<IdentityUserToken<int>>().ToTable("benutzertokens");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("rollenclaims");

            builder.Entity<Mandant>().ToTable("mandanten");
            builder.Entity<MandantenSkr>().ToTable("mandanten_skr");

            builder.Entity<MandantenSkr>()
                .HasOne(ms => ms.Mandant)
                .WithMany(m => m.SkrZuordnungen)
                .HasForeignKey(ms => ms.MandantId);

            builder.Entity<MandantenSkr>()
                .Property(ms => ms.MandantId)
                .IsRequired();
        }
    }
}
