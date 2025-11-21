using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ShadowViewer.Plugin.Bika.Constants;
using ShadowViewer.Sdk.Models.Interfaces;

namespace ShadowViewer.Plugin.Bika.Models;

public partial class BikaNavigationItem: ObservableObject,IShadowNavigationItem
{
    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty] private object? content;
    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty] private IconElement? icon;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string? Id { get; set; }

    public string PluginId => PluginConstants.PluginId;
}