using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FluntIcon;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using PicaComic;
using ShadowViewer.Controls;
using ShadowViewer.Enums;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;

namespace ShadowViewer.Plugin.Bika.ViewModels;

public partial class BikaSettingsViewModel : ObservableObject
{
    private IPicaClient BikaClient { get; }

     
    public BikaSettingsViewModel(IPicaClient client)
    {
        BikaClient = client;
        
    }

    [ObservableProperty] private bool pingShow;
    [ObservableProperty] private int apiShunt= IPicaClient.AppChannel - 1;
    [ObservableProperty] private int picShunt= IPicaClient.FileChannel - 1;
    [ObservableProperty] private string pingText;
    [ObservableProperty] private bool canTemporaryUnlock = BikaConfig.CanTemporaryUnlockComic;
    [ObservableProperty] private bool loadLockComic = BikaConfig.LoadLockComic;
    [ObservableProperty] private bool isIgnoreLockComic = BikaConfig.IsIgnoreLockComic;
    [ObservableProperty] private SolidColorBrush pingColor = new(Colors.Green);
    [ObservableProperty] private FluentIconSymbol pingIcon = FluentIconSymbol.CheckmarkCircleFilled;
    private ICallableService Caller { get; }

    public BikaSettingsViewModel(ICallableService callableService)
    {
        Caller = callableService;
        LoadLockComicShow = !BikaConfig.IsIgnoreLockComic;
        CanTemporaryUnlockShow = !BikaConfig.IsIgnoreLockComic && BikaConfig.LoadLockComic;
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
            BikaConfig.LoadLockComic = newValue;
            CanTemporaryUnlockShow = !IsIgnoreLockComic && LoadLockComic;
        }
    }

    partial void OnIsIgnoreLockComicChanged(bool oldValue, bool newValue)
    {
        if (oldValue != newValue)
        {
            BikaConfig.IsIgnoreLockComic = newValue;
            LoadLockComicShow = !IsIgnoreLockComic;
            CanTemporaryUnlockShow = !IsIgnoreLockComic && LoadLockComic;
        }
    }

    partial void OnCanTemporaryUnlockChanged(bool oldValue, bool newValue)
    {
        if (oldValue != newValue) BikaConfig.CanTemporaryUnlockComic = newValue;
    }

    partial void OnApiShuntChanged(int oldValue, int newValue)
    {
        if (oldValue != newValue)
        {
            IPicaClient.AppChannel = newValue + 1;
            BikaConfigHelper.Set(BikaConfigKey.ApiShunt, IPicaClient.AppChannel);
        }
    }

    partial void OnPicShuntChanged(int oldValue, int newValue)
    {
        if (oldValue != newValue)
        {
            IPicaClient.FileChannel = newValue + 1;
            BikaConfigHelper.Set(BikaConfigKey.PicShunt, IPicaClient.FileChannel);
        }
    }

    #endregion

    public async Task Ping()
    {
        PingShow = true;
        PingText = BikaResourcesHelper.GetString(BikaResourceKey.Pinging);
        PingIcon = FluentIconSymbol.ArrowSyncCircleFilled;
        PingColor = new SolidColorBrush(Colors.Orange);
        try
        {
            var ms = await BikaClient.PingBaseUrl();
            PingText = $"{ms}ms";
            PingIcon = FluentIconSymbol.CheckmarkCircleFilled;
            PingColor = new SolidColorBrush(Colors.Green);
        }
        catch (Exception)
        {
            PingText = BikaResourcesHelper.GetString(BikaResourceKey.TimeOut);
            PingIcon = FluentIconSymbol.DismissCircleFilled;
            PingColor = new SolidColorBrush(Colors.Red);
        }
    }

    public void SetProxy(string text)
    {
        try
        {
            var uri = new Uri(text);
            BikaClient.SetProxy(uri);
            BikaConfig.Proxy = text;
            Caller.TopGrid(this, new TipPopup(
                $"[{BikaPlugin.Meta.Name}]{BikaResourcesHelper.GetString(BikaResourceKey.Proxy)}({text}){BikaResourcesHelper.GetString(BikaResourceKey.SetSuccess)}",
                InfoBarSeverity.Success), TopGridMode.Tip);
        }
        catch (Exception)
        {
            Caller.TopGrid(this, new TipPopup(
                $"[{BikaPlugin.Meta.Name}]{BikaResourcesHelper.GetString(BikaResourceKey.Proxy)}({text}){BikaResourcesHelper.GetString(BikaResourceKey.SetError)}",
                InfoBarSeverity.Error), TopGridMode.Tip);
        }
    }
    
    
}