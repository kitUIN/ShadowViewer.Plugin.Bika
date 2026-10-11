using System;
using System.Linq;
using DryIoc;
using Microsoft.EntityFrameworkCore;
using PicaComic.Models;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.Bika.Constants;
using ShadowViewer.Plugin.Local.Database;
using ShadowViewer.Plugin.Local.Models;
using ShadowViewer.Sdk.Database;

namespace ShadowViewer.Plugin.Bika.Helpers;

public static class BikaHistoryHelper
{
    public static void Add(ComicInfo? comic)
    {
        if (comic == null) return;
        using var db = DiFactory.Services.Resolve<IDbContextFactory<LocalDbContext>>().CreateDbContext();
        var history = db.Set<LocalHistory>().FirstOrDefault(x =>
            x.PluginId == PluginConstants.PluginId && x.Extra == comic.Id);
        if (history == null)
        {
            history = new LocalHistory
            {
                Id = DatabaseIds.Next(),
                PluginId = PluginConstants.PluginId,
                Extra = comic.Id
            };
            db.Add(history);
        }
        history.Thumb = comic.Thumb.FilePath;
        history.Title = comic.Title;
        history.LastReadDateTime = DateTime.Now;
        db.SaveChanges();
    }
}
