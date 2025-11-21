using ShadowViewer.Plugin.Bika.Constants;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Plugin.Local.Models;
using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Models;

/// <summary>
/// 
/// </summary>
[SugarTable(nameof(LocalHistory))]
public partial class BikaHistory: LocalHistory
{
    /// <summary>
    /// <inheritdoc cref="IHistory.PluginId"/>
    /// </summary>
    public override string PluginId { get; set; } = PluginConstants.PluginId;
}