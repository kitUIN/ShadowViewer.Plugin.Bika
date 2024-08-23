using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ShadowViewer.Interfaces;
using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Models;

public partial class BikaHistory: ObservableObject,IHistory
{
    [ObservableProperty][property:SugarColumn(IsPrimaryKey = true)] private string id;
    [ObservableProperty] private string? title;
    [ObservableProperty] private string? icon;
    [ObservableProperty] private DateTime time;
    [SugarColumn(IsIgnore = true)]
    public string Plugin => BikaPlugin.Meta.Id;

    public string PluginId => BikaPlugin.Meta.Id;
}