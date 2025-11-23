using DryIoc;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.Bika.Configs;

namespace ShadowViewer.Plugin.Bika.Helpers;

/// <summary>
/// 
/// </summary>
public static class VersionHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string GetPluginVersion() => DiFactory.Services.Resolve<BikaPlugin>().MetaData.Version.ToString();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string GetLowestVersion() => DiFactory.Services.Resolve<BikaPlugin>().MetaData.SdkVersion.ToString();
}