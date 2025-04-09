using System;
using DryIoc;
using PicaComic.Models;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Models.Interfaces;
using ShadowViewer.Plugin.Bika.Models;
using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaHistoryHelper
{
    public static void Add(ComicInfo? comic)
    {
        if (comic == null) return;
        var obj = new BikaHistory()
        {
            Extra = comic.Id,
            LastReadDateTime = DateTime.Now,
            Thumb = comic.Thumb.FilePath,
            Title = comic.Title,
        };
        DiFactory.Services.Resolve<ISqlSugarClient>().Storageable(obj).ExecuteCommand();
    }
    public static void Add(IHistory history)
    {
        DiFactory.Services.Resolve<ISqlSugarClient>().Storageable(new BikaHistory()
        {
            Id = history.Id,
            LastReadDateTime = DateTime.Now,
            Thumb = history.Thumb,
            Title = history.Title,
        }).ExecuteCommand();
    }
}