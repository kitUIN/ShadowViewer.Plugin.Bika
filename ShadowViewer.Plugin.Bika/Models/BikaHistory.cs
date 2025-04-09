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
    [SugarColumn(IsIgnore = true)]
    public new string PluginId => BikaPlugin.Meta.Id;
}