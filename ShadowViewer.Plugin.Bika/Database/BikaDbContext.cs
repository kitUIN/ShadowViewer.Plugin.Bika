using Microsoft.EntityFrameworkCore;
using ShadowViewer.Plugin.Bika.Models;

namespace ShadowViewer.Plugin.Bika.Database;

/// <summary>Maps the existing Bika account table without changing SDK-owned tables.</summary>
public sealed class BikaDbContext(DbContextOptions<BikaDbContext> options) : DbContext(options)
{
    public DbSet<BikaUser> Users => Set<BikaUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<BikaUser>();
        user.ToTable("BikaUser");
        user.HasKey(x => x.Email);
        user.Property(x => x.Email).HasMaxLength(2048);
        user.Property(x => x.Password).HasMaxLength(2048);
        user.Property(x => x.Token).HasMaxLength(2048);
    }

    public void Initialize()
    {
        // EnsureCreated skips plugin tables when the shared database already contains tables.
        Database.ExecuteSqlRaw("""
            CREATE TABLE IF NOT EXISTS "BikaUser" (
                "Email" TEXT NOT NULL PRIMARY KEY,
                "Password" TEXT NOT NULL,
                "Token" TEXT NOT NULL
            );
            """);
    }
}
