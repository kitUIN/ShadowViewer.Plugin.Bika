using System.Collections.Generic;
using System.Linq;
using PicaComic;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Core.Args;
using ShadowViewer.Core.Plugins;
using ShadowViewer.Core.Responders;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Local.ViewModels;

using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Responders;

[EntryPoint(Name = nameof(PluginResponder.PicViewResponder))]
public partial class BikaPicViewResponder: AbstractPicViewResponder
{
    [Autowired]
    public IPicaClient Client { get; }

    public override void PicturesLoadStarting(object sender, PicViewArg e)
    {
        if (sender is not PicViewModel viewModel) return;
        if (e.Affiliation != Id || e.Parameter is not ComicArg arg) return;
        var orders = new List<int>();
        foreach (var episode in arg.Episodes.Reverse())
        {
            orders.Add(episode.Order);
            viewModel.Episodes.Add(
                new BikaEpisode(episode,arg.ComicInfo.Id));
        }

        if (viewModel.CurrentEpisodeIndex == -1 && orders.Count > 0)
            viewModel.CurrentEpisodeIndex = orders.IndexOf(arg.CurrentEpisode);
    }

    public override async void CurrentEpisodeIndexChanged(object sender, string affiliation, int oldValue, int newValue)
    {
        if (sender is not PicViewModel viewModel) return;
        if (oldValue == newValue) return;
        if (viewModel.Affiliation != Id) return;
        viewModel.Images.Clear();
        var index = 0;
        if (viewModel.Episodes.Count > 0 && viewModel.Episodes[newValue] is BikaEpisode episode)
        {
            var pages = 1;
            var page = 1;
            await BikaHttpHelper.TryRequest(this, Client.Pictures(episode.ComicId, episode.Order, page), res =>
            {
                pages = res.Data.Pages.Pages;
                foreach (var item in res.Data.Pages.Docs)
                {
                    viewModel.Images.Add(new BikaPicture(++index, item.Media.FilePath));
                }
            });
            for (var i = 2; i <= pages; i++)
            {
                await BikaHttpHelper.TryRequest(this, Client.Pictures(episode.ComicId, episode.Order, page), res =>
                {
                    foreach (var item in res.Data.Pages.Docs)
                    {
                        viewModel.Images.Add(new BikaPicture(++index, item.Media.FilePath));
                    }
                });
            }
        }
    }
 
}