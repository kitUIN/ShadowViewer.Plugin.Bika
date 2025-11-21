using Microsoft.UI.Xaml.Controls;
using PicaComic;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Constants;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Bika.Pages;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Plugins;
using ShadowViewer.Sdk.Responders;
using ShadowViewer.Sdk.Services;
using System.Collections.Generic;

namespace ShadowViewer.Plugin.Bika.Responders;
/// <summary>
/// 
/// </summary>
[EntryPoint(Name = nameof(PluginResponder.SearchSuggestionResponder))]
public partial class BikaSearchResponder: AbstractSearchSuggestionResponder
{
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    public IPicaClient BikaClient { get; }
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    public INavigateService NavigateService { get; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IEnumerable<IShadowSearchItem> SearchTextChanged(AutoSuggestBox sender,
        AutoSuggestBoxTextChangedEventArgs args)
    {
        var res = new List<IShadowSearchItem>();
        if (!string.IsNullOrEmpty(sender.Text) && BikaClient.HasToken)
        {
            res.Add(new BikaSearchItem(sender.Text, PluginConstants.PluginId, BikaSearchMode.Search));
        }

        return res;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void SearchSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void SearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        BikaSearchItem? item = null;
        if (args.ChosenSuggestion is BikaSearchItem item1)
        {
            item = item1;
        }
        else if (args.ChosenSuggestion == null && sender.Items[0] is BikaSearchItem item2)
        {
            item = item2;
        }

        if (item != null)
        {
            NavigateService.Navigate(typeof(BikaCategoryPage),
                new CategoryArg
                {
                    Category = item.Title,
                    Mode = CategoryMode.Search
                });
        }
    }
}