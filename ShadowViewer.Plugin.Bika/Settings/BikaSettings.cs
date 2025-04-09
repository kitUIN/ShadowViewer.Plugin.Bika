using System;
using DryIoc;
using PicaComic;
using Serilog;
using ShadowPluginLoader.WinUI;

namespace ShadowViewer.Plugin.Bika.Settings;
/// <summary>
/// 
/// </summary>
public partial class BikaSettings
{
    
    /// <summary>
    /// 修改代理时触发
    /// </summary> 
    partial void ProxyChanged(string newValue)
    {
        if (string.IsNullOrEmpty(newValue)) return;
        try
        {
            DiFactory.Services.Resolve<IPicaClient>().SetProxy(new Uri(newValue));
        }
        catch (Exception ex)
        {
            Log.Error($"{ex}", ex);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    partial void ApiShuntChanged(int newValue)
    {
        IPicaClient.AppChannel = newValue;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newValue"></param>
    partial void PicShuntChanged(int newValue)
    {
        IPicaClient.FileChannel = newValue;
    }

    partial void AfterInit()
    {
        var client = DiFactory.Services.Resolve<IPicaClient>();
        IPicaClient.AppChannel = ApiShunt;
        IPicaClient.FileChannel = PicShunt;
        if (!string.IsNullOrEmpty(Proxy))
            client.SetProxy(new Uri(Proxy));
    }
}