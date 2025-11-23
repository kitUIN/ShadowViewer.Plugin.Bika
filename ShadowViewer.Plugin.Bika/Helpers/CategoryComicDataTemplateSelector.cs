using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PicaComic.Models;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.Bika.Configs;

namespace ShadowViewer.Plugin.Bika.Helpers;

/// <summary>
/// 
/// </summary>
public class CategoryComicDataTemplateSelector : DataTemplateSelector
{
    /// <summary>
    /// 
    /// </summary>
    public DataTemplate DontLoadTemplate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DataTemplate LoadTemplate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public BikaPluginConfig Config { get; } = DiFactory.Services.Resolve<BikaPluginConfig>();

    /// <inheritdoc />
    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is not CategoryComic category)
        {
            return LoadTemplate;
        }

        return !Config.LoadLockComic && category.IsLocked ? DontLoadTemplate : LoadTemplate;
    }

    /// <inheritdoc />
    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        return SelectTemplateCore(item);
    }
}