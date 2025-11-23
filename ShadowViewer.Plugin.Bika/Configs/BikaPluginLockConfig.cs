using System.Collections.Generic;
using ShadowObservableConfig.Attributes;

namespace ShadowViewer.Plugin.Bika.Configs;

/// <summary>
/// 
/// </summary>
[ObservableConfig(FileName = "bika_plugin_lock", FileExt = ".json")]
public partial class BikaPluginLockConfig
{
    /// <summary>
    /// 封印设置
    /// </summary>
    [ObservableConfigProperty(Description = "封印设置")]
    private Dictionary<string, bool> locks = new();
}