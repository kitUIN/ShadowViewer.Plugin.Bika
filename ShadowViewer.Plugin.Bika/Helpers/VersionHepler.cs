using ShadowPluginLoader.WinUI;

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
    public static string LowestVersion { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    public static string PluginVersion { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="metaData"></param>
    public static void Init(AbstractPluginMetaData metaData)
    {
        LowestVersion = metaData.SdkVersion.ToString();
        PluginVersion = metaData.Version.ToString();
    }
}