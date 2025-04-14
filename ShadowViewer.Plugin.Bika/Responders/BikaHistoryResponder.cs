using System;
using System.Collections.Generic;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Core;
using ShadowViewer.Core.Enums;
using ShadowViewer.Core.Models.Interfaces;
using ShadowViewer.Core.Plugins;
using ShadowViewer.Core.Responders;
using ShadowViewer.Core.Services;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Bika.Pages;
using ShadowViewer.Plugin.Local.Models;
using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Responders;

[EntryPoint(Name = nameof(PluginResponder.HistoryResponder))]
public partial class BikaHistoryResponder : AbstractHistoryResponder
{
    [Autowired] public ISqlSugarClient Db { get; }
    [Autowired] public INavigateService NavigateService { get; }

    public override IEnumerable<IHistory> GetHistories(HistoryMode mode = HistoryMode.Day) =>
        mode switch
        {
            HistoryMode.Day => Db.Queryable<BikaHistory>()
                .Where(history => history.LastReadDateTime >= DateTime.Now - TimeSpan.FromDays(1))
                .ToList(),
            HistoryMode.Week => Db.Queryable<BikaHistory>()
                .Where(history => history.LastReadDateTime >= DateTime.Now - TimeSpan.FromDays(7))
                .ToList(),
            HistoryMode.Month => Db.Queryable<BikaHistory>()
                .Where(history => history.LastReadDateTime >= DateTime.Now - TimeSpan.FromDays(30))
                .ToList(),
            _ => Db.Queryable<BikaHistory>().ToList()
        };

    public override void ClickHistoryHandler(IHistory history)
    {
        NavigateService.Navigate(typeof(BikaInfoPage), history.Extra!, force: true);
    }

    public override void DeleteHistoryHandler(IHistory history)
    {
        Db.Deleteable(new BikaHistory { Id = history.Id }).ExecuteCommand();
    }
}