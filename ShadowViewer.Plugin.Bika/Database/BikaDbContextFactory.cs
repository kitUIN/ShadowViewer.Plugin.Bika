using Microsoft.EntityFrameworkCore;
using ShadowViewer.Sdk.Database;

namespace ShadowViewer.Plugin.Bika.Database;

public sealed class BikaDbContextFactory(IDbContextFactory<ShadowDbContext> sdkFactory)
    : IDbContextFactory<BikaDbContext>
{
    public BikaDbContext CreateDbContext()
    {
        using var sdk = sdkFactory.CreateDbContext();
        return new BikaDbContext(new DbContextOptionsBuilder<BikaDbContext>()
            .UseSqlite(sdk.Database.GetConnectionString()).Options);
    }
}
