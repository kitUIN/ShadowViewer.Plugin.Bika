using System;
using DryIoc;
using PicaComic.Models;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Models;
using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaHistoryHelper
{
    public static void Add(Comic comic)
    {
        DiFactory.Services.Resolve<ISqlSugarClient>().Storageable(new BikaHistory()
        {
            Id = comic.Id,
            Time = DateTime.Now,
            Icon = comic.Thumb.FilePath,
            Title = comic.Title,
        }).ExecuteCommand();
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