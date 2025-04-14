using System;
using DryIoc;
using PicaComic.Models;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.Bika.Models;
using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaHistoryHelper
{
    public static void Add(ComicInfo? comic)
    {
        var db = DiFactory.Services.Resolve<ISqlSugarClient>();
        if (comic == null) return;
        var history = db.Queryable<BikaHistory>().Where(x => x.Extra == comic.Id).First();
        if (history != null)
        {
            db.Updateable<BikaHistory>()
                .SetColumns(it => it.Thumb == comic.Thumb.FilePath)
                .SetColumns(it => it.Title == comic.Title)
                .SetColumns(it => it.LastReadDateTime == DateTime.Now)
                .Where(x => history.Id == x.Id)
                .ExecuteCommand();
        }
        else
        {
            db.Insertable(new BikaHistory()
            {
                Id = SnowFlakeSingle.Instance.NextId(),
                Extra = comic.Id,
                LastReadDateTime = DateTime.Now,
                Thumb = comic.Thumb.FilePath,
                Title = comic.Title,
            }).ExecuteCommand();
        }
    }
}