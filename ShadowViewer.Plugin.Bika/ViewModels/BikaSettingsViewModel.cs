using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using PicaComic;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Plugin.Bika.Configs;
using ShadowViewer.Sdk.Services;
using System;
using System.Threading.Tasks;

namespace ShadowViewer.Plugin.Bika.ViewModels;

[CheckAutowired]
public partial class BikaSettingsViewModel : ObservableObject
{
    private IPicaClient BikaClient { get; }


    /// <summary>
    /// Config
    /// </summary>
    [Autowired]
    public BikaPluginConfig Config { get; }
    [ObservableProperty] private bool pingShow;
    [ObservableProperty] private int apiShunt= IPicaClient.AppChannel - 1;
    [ObservableProperty] private int picShunt= IPicaClient.FileChannel - 1;
    [ObservableProperty] private string pingText;
    [ObservableProperty] private bool canTemporaryUnlock; //  = Config.CanTemporaryUnlockComic;
    [ObservableProperty] private bool loadLockComic; //  = Config.LoadLockComic;
    [ObservableProperty] private bool isIgnoreLockComic; // = Config.IsIgnoreLockComic;
    [ObservableProperty] private SolidColorBrush pingColor = new(Colors.Green);
    // [ObservableProperty] private FluentFilledIconSymbol pingIcon = FluentFilledIconSymbol.CheckmarkCircle20Filled;
    private ICallableService Caller { get; }

    public BikaSettingsViewModel(ICallableService callableService, IPicaClient client)
    {
        BikaClient = client;
        Caller = callableService;
        LoadLockComicShow = !Config.IsIgnoreLockComic;
        CanTemporaryUnlockShow = !Config.IsIgnoreLockComic && Config.LoadLockComic;
    }
    public void ResetProxy()
    {
        BikaClient.ResetProxy();
    }
    private bool loadLockComicShow;

    public bool LoadLockComicShow
    {
        get => loadLockComicShow;
        set => SetProperty(ref loadLockComicShow, value);
    }

    private bool canTemporaryUnlockShow;

    public bool CanTemporaryUnlockShow
    {
        get => canTemporaryUnlockShow;
        set => SetProperty(ref canTemporaryUnlockShow, value);
    }

    #region Changed

    partial void OnLoadLockComicChanged(bool oldValue, bool newValue)
    {
        if (oldValue != newValue)
        {
            Config.LoadLockComic = newValue;
            CanTemporaryUnlockShow = !IsIgnoreLockComic && LoadLockComic;
        }
    }

    partial void OnIsIgnoreLockComicChanged(bool oldValue, bool newValue)
    {
        if (oldValue != newValue)
        {
            Config.IsIgnoreLockComic = newValue;
            LoadLockComicShow = !IsIgnoreLockComic;
            CanTemporaryUnlockShow = !IsIgnoreLockComic && LoadLockComic;
        }
    }

    partial void OnCanTemporaryUnlockChanged(bool oldValue, bool newValue)
    {
        if (oldValue != newValue) Config.CanTemporaryUnlockComic = newValue;
    }


    #endregion

    public async Task Ping()
    {
        // PingShow = true;
        // PingText = ResourcesHelper.GetString(ResourceKey.Pinging);
        // PingIcon = FluentFilledIconSymbol.ArrowSyncCircle20Filled;
        // PingColor = new SolidColorBrush(Colors.Orange);
        // try
        // {
        //     var ms = await BikaClient.PingBaseUrl();
        //     PingText = $"{ms}ms";
        //     PingIcon = FluentFilledIconSymbol.CheckmarkCircle20Filled;
        //     PingColor = new SolidColorBrush(Colors.Green);
        // }
        // catch (Exception)
        // {
        //     PingText = ResourcesHelper.GetString(ResourceKey.TimeOut);
        //     PingIcon = FluentFilledIconSymbol.DismissCircle20Filled;
        //     PingColor = new SolidColorBrush(Colors.Red);
        // }
    }

    public void SetProxy(string text)
    {
        try
        {
            var uri = new Uri(text);
            BikaClient.SetProxy(uri);
            Config.Proxy = text;
            //NotificationHelper.Notify(this,$"{ResourcesHelper.GetString(ResourceKey.Proxy)}({text}){ResourcesHelper.GetString(ResourceKey.SetSuccess)}",
            //    InfoBarSeverity.Success);
        }
        catch (Exception)
        {
            //NotificationHelper.Notify(this,$"{ResourcesHelper.GetString(ResourceKey.Proxy)}({text}){ResourcesHelper.GetString(ResourceKey.SetError)}",
            //    InfoBarSeverity.Success);
        }
    }

}