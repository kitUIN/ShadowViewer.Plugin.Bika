using System;
using System.Collections.Generic;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Sdk;
using ShadowViewer.Sdk.Enums;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Plugins;
using ShadowViewer.Sdk.Responders;
using ShadowViewer.Sdk.Services;
using ShadowViewer.Plugin.Bika.Pages;
using ShadowViewer.Plugin.Local.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ShadowViewer.Plugin.Local.Database;
using ShadowViewer.Plugin.Bika.Constants;

namespace ShadowViewer.Plugin.Bika.Responders;

[EntryPoint(Name = nameof(PluginResponder.HistoryResponder))]
public partial class BikaHistoryResponder : AbstractHistoryResponder
{
    [Autowired] public IDbContextFactory<LocalDbContext> DbFactory { get; }
    [Autowired] public INavigateService NavigateService { get; }

    public override IEnumerable<IHistory> GetHistories(HistoryMode mode = HistoryMode.Day)
    {
        using var db = DbFactory.CreateDbContext();
        var since = mode switch
        {
            HistoryMode.Day => DateTime.Now.AddDays(-1),
            HistoryMode.Week => DateTime.Now.AddDays(-7),
            HistoryMode.Month => DateTime.Now.AddDays(-30),
            _ => DateTime.MinValue
        };
        return db.Set<LocalHistory>().AsNoTracking()
            .Where(x => x.PluginId == PluginConstants.PluginId && x.LastReadDateTime >= since)
            .OrderByDescending(x => x.LastReadDateTime).ToList();
    }

    public override void ClickHistoryHandler(IHistory history)
    {
        NavigateService.Navigate(typeof(BikaInfoPage), history.Extra!, force: true);
    }

    public override void DeleteHistoryHandler(IHistory history)
    {
        using var db = DbFactory.CreateDbContext();
        db.Set<LocalHistory>().Where(x => x.Id == history.Id && x.PluginId == PluginConstants.PluginId)
            .ExecuteDelete();
    }
}
