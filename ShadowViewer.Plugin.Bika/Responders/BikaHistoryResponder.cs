using System;
using System.Collections.Generic;
using ShadowViewer.Enums;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Bika.Pages;
using ShadowViewer.Responders;
using ShadowViewer.Services;

using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Responders;

public class BikaHistoryResponder : AbstractHistoryResponder
{
    public override IEnumerable<IHistory> GetHistories(HistoryMode mode = HistoryMode.Day)
    {
        return mode switch
        {
            HistoryMode.Day => Db.Queryable<BikaHistory>()
                .Where(history => history.Time >= DateTime.Now - TimeSpan.FromDays(1))
                .ToList(),
            HistoryMode.Week => Db.Queryable<BikaHistory>()
                .Where(history => history.Time >= DateTime.Now - TimeSpan.FromDays(7))
                .ToList(),
            HistoryMode.Month => Db.Queryable<BikaHistory>()
                .Where(history => history.Time >= DateTime.Now - TimeSpan.FromDays(30))
                .ToList(),
            _ => Db.Queryable<BikaHistory>().ToList()
        };
    }

    public override void ClickHistoryHandler(IHistory history)
    {
        Caller.NavigateTo(typeof(BikaInfoPage), history.Id);
        BikaHistoryHelper.Add(history);
    }

    public override void DeleteHistoryHandler(IHistory history)
    {
        Db.Deleteable(new BikaHistory { Id = history.Id }).ExecuteCommand();
    }

    public BikaHistoryResponder(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, PluginLoader pluginService, string id) : base(callableService,
        sqlSugarClient, compressServices, pluginService, id)
    {
    }
}