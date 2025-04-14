using ShadowViewer.Core.Models.Interfaces;
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
    public override string PluginId { get; set; } = BikaPlugin.Meta.Id;
}