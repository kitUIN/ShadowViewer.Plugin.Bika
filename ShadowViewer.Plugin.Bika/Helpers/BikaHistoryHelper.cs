using System;
using DryIoc;
using PicaComic.Models;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Models;
using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaHistoryHelper
{
    public static void Add(ComicInfo? comic)
    {
        if(comic != null)
        {
            var obj = new BikaHistory()
            {
                Id = comic.Id,
                Time = DateTime.Now,
                Icon = comic.Thumb.FilePath,
                Title = comic.Title,
            };
            DiFactory.Services.Resolve<ISqlSugarClient>().Storageable(obj).ExecuteCommand();
        }
    }
    public static void Add(IHistory history)
    {
        DiFactory.Services.Resolve<ISqlSugarClient>().Storageable(new BikaHistory()
        {
            Id = history.Id,
            Time = DateTime.Now,
            Icon = history.Icon,
            Title = history.Title,
        }).ExecuteCommand();
    }
}