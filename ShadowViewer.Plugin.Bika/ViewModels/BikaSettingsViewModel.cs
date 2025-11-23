using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using PicaComic;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Plugin.Bika.Configs;
using ShadowViewer.Sdk.Services;
using System;
using System.Threading.Tasks;
using ShadowViewer.Plugin.Bika.I18n;

namespace ShadowViewer.Plugin.Bika.ViewModels;

[CheckAutowired]
public partial class BikaSettingsViewModel : ObservableObject
{
    /// <summary>
    /// BikaClient
    /// </summary>
    [Autowired] private IPicaClient BikaClient { get; }

    /// <summary>
    /// Config
    /// </summary>
    [Autowired]
    public BikaPluginConfig Config { get; }

    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    private ICallableService Caller { get; }

    /// <summary>
    /// Ping Show
    /// </summary>
    [ObservableProperty] private bool pingShow;

    /// <summary>
    /// Ping Text
    /// </summary>
    [ObservableProperty] private string pingText;

    /// <summary>
    /// Ping Color
    /// </summary>
    [ObservableProperty] private SolidColorBrush pingColor = new(Colors.Green);

    /// <summary>
    /// Ping Icon
    /// </summary>
    [ObservableProperty] private FluentIcons.Common.Icon pingIcon = FluentIcons.Common.Icon.CheckmarkCircle;
 
    /// <summary>
    ///  Ping Button
    /// </summary>
    /// <returns></returns>
    public async Task Ping()
    {
        PingShow = true;
        PingText = ResourcesHelper.GetString(ResourceKey.Pinging);
        PingIcon = FluentIcons.Common.Icon.ArrowSyncCircle;
        PingColor = new SolidColorBrush(Colors.Orange);
        try
        {
            var ms = await BikaClient.PingBaseUrl();
            PingText = $"{ms}ms";
            PingIcon = FluentIcons.Common.Icon.CheckmarkCircle;
            PingColor = new SolidColorBrush(Colors.Green);
        }
        catch (Exception)
        {
            PingText = ResourcesHelper.GetString(ResourceKey.TimeOut);
            PingIcon = FluentIcons.Common.Icon.DismissCircle;
            PingColor = new SolidColorBrush(Colors.Red);
        }
    }
}