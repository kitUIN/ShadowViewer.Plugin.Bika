using CommunityToolkit.Mvvm.ComponentModel;
using DryIoc;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.Bika.Configs;

namespace ShadowViewer.Plugin.Bika.Models;
/// <summary>
/// 
/// </summary>
public partial class BikaLock : ObservableObject
{
    /// <summary>
    /// 
    /// </summary>
    private BikaPluginLockConfig lockConfig = DiFactory.Services.Resolve<BikaPluginLockConfig>();
    /// <summary>
    /// 
    /// </summary>
    [ObservableProperty] private string title;
    /// <summary>
    /// 
    /// </summary>
    [ObservableProperty] private bool isOpened;
    /// <summary>
    /// 
    /// </summary>
    private string icon;
    /// <summary>
    /// 
    /// </summary>
    public string Icon
    {
        get => icon;
        set => SetProperty(ref icon, value);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="isOpened"></param>
    public BikaLock(string title, bool isOpened)
    {
        Title = title;
        IsOpened = isOpened;
        Icon = IsOpened ? "\uE785" : "\uE72E";
    }

    partial void OnIsOpenedChanged(bool oldValue, bool newValue)
    {
        if (oldValue != newValue)
        {
            Icon = IsOpened ? "\uE785" : "\uE72E";
            lockConfig.Locks[title] = newValue;
        }
    }
}