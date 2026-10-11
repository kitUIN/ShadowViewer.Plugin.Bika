using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShadowViewer.Plugin.Bika.Database;
using ShadowViewer.Plugin.Bika.Models;

using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();
var options = new DbContextOptionsBuilder<BikaDbContext>().UseSqlite(connection).Options;
using (var db = new BikaDbContext(options))
{
    db.Database.ExecuteSqlRaw("CREATE TABLE SdkSentinel (Id INTEGER PRIMARY KEY); INSERT INTO SdkSentinel VALUES (42);");
    db.Initialize();
    db.Users.Add(new BikaUser { Email = "test@example.invalid", Password = "test", Token = "old" });
    db.SaveChanges();
}
using (var db = new BikaDbContext(options))
{
    db.Initialize();
    var user = await db.Users.FindAsync("test@example.invalid") ?? throw new Exception("Lost existing account");
    if (user.Password != "test" || user.Token != "old") throw new Exception("Changed existing account");
    user.Token = "new";
    await db.SaveChangesAsync();
}
using (var db = new BikaDbContext(options))
{
    if (await db.Users.CountAsync() != 1 || (await db.Users.SingleAsync()).Token != "new")
        throw new Exception("Account update failed");
    using var command = connection.CreateCommand();
    command.CommandText = "SELECT Id FROM SdkSentinel";
    if ((long)command.ExecuteScalar()! != 42) throw new Exception("SDK tables modified");
}
Console.WriteLine("PASS shared database initialization, account preservation, token update, and context disposal");

using (var db = new ShadowViewer.Plugin.Local.Database.LocalDbContext(
    new DbContextOptionsBuilder<ShadowViewer.Plugin.Local.Database.LocalDbContext>().UseSqlite(connection).Options))
{
    db.Database.ExecuteSqlRaw("""
        CREATE TABLE LocalHistory (Id INTEGER PRIMARY KEY, Title TEXT NOT NULL, Thumb TEXT NOT NULL,
            LastReadDateTime TEXT NOT NULL, Extra TEXT NULL, PluginId TEXT NOT NULL);
        """);
    db.AddRange(new ShadowViewer.Plugin.Local.Models.LocalHistory
        { Id = 1, Title = "Local", Thumb = "local", LastReadDateTime = DateTime.Now },
        new ShadowViewer.Plugin.Local.Models.LocalHistory
        { Id = 2, Title = "Bika", Thumb = "network", LastReadDateTime = DateTime.Now,
            PluginId = "ShadowViewer.Plugin.Bika", Extra = "comic-id" });
    db.SaveChanges();
}
var factory = new HistoryFactory(connection);
var bika = new ShadowViewer.Plugin.Bika.Responders.BikaHistoryResponder(factory);
var local = new ShadowViewer.Plugin.Local.Responders.LocalHistoryResponder(factory);
var bikaHistory = bika.GetHistories().Single();
var localHistory = local.GetHistories().Single();
if (bikaHistory.Id != 2 || localHistory.Id != 1) throw new Exception("Histories leaked between plugins");
bika.DeleteHistoryHandler(localHistory);
local.DeleteHistoryHandler(bikaHistory);
if (bika.GetHistories().Count() != 1 || local.GetHistories().Count() != 1)
    throw new Exception("Cross-plugin deletion was allowed");
bika.DeleteHistoryHandler(bikaHistory);
if (bika.GetHistories().Any() || local.GetHistories().Count() != 1)
    throw new Exception("Bika deletion affected local history");
Console.WriteLine("PASS actual Bika and Local responders isolate shared history queries and deletions");

sealed class HistoryFactory(SqliteConnection connection)
    : IDbContextFactory<ShadowViewer.Plugin.Local.Database.LocalDbContext>
{
    public ShadowViewer.Plugin.Local.Database.LocalDbContext CreateDbContext() => new(
        new DbContextOptionsBuilder<ShadowViewer.Plugin.Local.Database.LocalDbContext>().UseSqlite(connection).Options);
}
