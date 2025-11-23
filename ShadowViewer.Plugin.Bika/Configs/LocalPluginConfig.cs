using System;
using PicaComic;
using Serilog;
using ShadowObservableConfig.Attributes;
using ShadowPluginLoader.WinUI;
using System.Reflection;
using DryIoc;

namespace ShadowViewer.Plugin.Bika.Configs;

[ObservableConfig(FileName = "bika_plugin_config")]
public partial class BikaPluginConfig
{
    /// <summary>
    /// Api分流
    /// </summary>
    [ObservableConfigProperty(Description = "Api分流")]
    private int apiShunt = 2;

    /// <summary>
    /// 图片分流
    /// </summary>
    [ObservableConfigProperty(Description = "图片分流")]
    private int picShunt = 2;

    /// <summary>
    /// 登录记住我
    /// </summary>
    [ObservableConfigProperty(Description = "登录记住我")]
    private bool rememberMe;

    /// <summary>
    /// 自动登录
    /// </summary>
    [ObservableConfigProperty(Description = "自动登录")]
    private bool autoLogin;

    /// <summary>
    /// 最后登录用户
    /// </summary>
    [ObservableConfigProperty(Description = "最后登录用户")]
    private string lastBikaUser;

    /// <summary>
    /// 代理
    /// </summary>
    [ObservableConfigProperty(Description = "代理")]
    private string? proxy;

    /// <summary>
    /// 被封印的漫画可以临时解封
    /// </summary>
    [ObservableConfigProperty(Description = "被封印的漫画可以临时解封")]
    private bool canTemporaryUnlockComic;

    /// <summary>
    /// 被封印的漫画可以临时解封 按钮能否修改
    /// </summary>
    [global::YamlDotNet.Serialization.YamlIgnore]
    public bool CanTemporaryUnlockShow => !IsIgnoreLockComic && LoadLockComic;

    /// <summary>
    /// 被封印的漫画直接忽略
    /// </summary>
    [ObservableConfigProperty(Description = "被封印的漫画直接忽略")]
    [ObservableConfigPropertyChangedFor(nameof(CanTemporaryUnlockShow))]
    private bool isIgnoreLockComic;

    /// <summary>
    /// 被封印的漫画加载
    /// </summary>
    [ObservableConfigProperty(Description = "被封印的漫画加载")]
    [ObservableConfigPropertyChangedFor(nameof(CanTemporaryUnlockShow))]
    private bool loadLockComic;


    /// <summary>
    /// 修改代理时触发
    /// </summary> 
    partial void AfterProxyChanged(string? oldValue, string? newValue)
    {
        if (oldValue != newValue) return;
        try
        {
            if (string.IsNullOrEmpty(newValue))
            {
                DiFactory.Services.Resolve<IPicaClient>().ResetProxy();
            }
            else
            {
                DiFactory.Services.Resolve<IPicaClient>().SetProxy(new Uri(newValue));
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{ex}", ex);
        }
    }
    // /// <summary>
    // /// 
    // /// </summary>
    // partial void ApiShuntChanged(int newValue)
    // {
    //     IPicaClient.AppChannel = newValue;
    // }
    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="newValue"></param>
    // partial void PicShuntChanged(int newValue)
    // {
    //     IPicaClient.FileChannel = newValue;
    // }
    //
    // partial void AfterInit()
    // {
    //     var client = DiFactory.Services.Resolve<IPicaClient>();
    //     IPicaClient.AppChannel = ApiShunt;
    //     IPicaClient.FileChannel = PicShunt;
    //     if (!string.IsNullOrEmpty(Proxy))
    //         client.SetProxy(new Uri(Proxy));
    // }
}