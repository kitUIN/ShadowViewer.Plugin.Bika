using System.Collections.Generic;
using System.Linq;
using PicaComic;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Sdk.Args;
using ShadowViewer.Sdk.Plugins;
using ShadowViewer.Sdk.Responders;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Local.ViewModels;

using System;
using System.Runtime.CompilerServices;

namespace ShadowViewer.Plugin.Bika.Responders;

[EntryPoint(Name = nameof(PluginResponder.PicViewResponder))]
public partial class BikaPicViewResponder: AbstractPicViewResponder
{
    private sealed class LoadState
    {
        public int Generation;
    }

    private readonly ConditionalWeakTable<PicViewModel, LoadState> loads = new();

    [Autowired]
    public IPicaClient Client { get; }

    public override void PicturesLoadStarting(object sender, PicViewContext ctx)
    {
        if (sender is not PicViewModel viewModel) return;
        if (ctx.Affiliation != Id || ctx.Parameter is not ComicArg arg) return;
        loads.GetOrCreateValue(viewModel).Generation++;
        var orders = new List<int>();
        foreach (var episode in arg.Episodes.Reverse())
        {
            orders.Add(episode.Order);
            viewModel.Episodes.Add(
                new BikaEpisode(episode,arg.ComicInfo.Id));
        }

        if (viewModel.CurrentEpisodeIndex == -1 && orders.Count > 0)
            viewModel.CurrentEpisodeIndex = Math.Max(0, orders.IndexOf(arg.CurrentEpisode));
    }
    public override async void CurrentEpisodeIndexChanged(object sender, PicViewContext ctx, int oldValue, int newValue)
    {
        if (sender is not PicViewModel viewModel) return;
        if (viewModel.Affiliation != Id) return;
        var state = loads.GetOrCreateValue(viewModel);
        var generation = ++state.Generation;
        if (oldValue == newValue || newValue < 0 || newValue >= viewModel.Episodes.Count) return;
        viewModel.Images.Clear();
        viewModel.CurrentPageIndex = 0;
        var index = 0;
        if (viewModel.Episodes.Count > 0 && viewModel.Episodes[newValue] is BikaEpisode episode)
        {
            var pages = 1;
            var page = 1;
            await BikaHttpHelper.TryRequest(this, Client.Pictures(episode.ComicId, episode.Order, page), res =>
            {
                if (state.Generation != generation || viewModel.CurrentEpisodeIndex != newValue) return;
                pages = res.Data.Pages.Pages;
                foreach (var item in res.Data.Pages.Docs)
                {
                    viewModel.Images.Add(new BikaPicture(++index, item.Media.FilePath));
                }
            });
            for (var i = 2; i <= pages && state.Generation == generation && viewModel.CurrentEpisodeIndex == newValue; i++)
            {
                await BikaHttpHelper.TryRequest(this, Client.Pictures(episode.ComicId, episode.Order, i), res =>
                {
                    if (state.Generation != generation || viewModel.CurrentEpisodeIndex != newValue) return;
                    foreach (var item in res.Data.Pages.Docs)
                    {
                        viewModel.Images.Add(new BikaPicture(++index, item.Media.FilePath));
                    }
                });
            }
        }
    }
 
}