using ShadowPluginLoader.Attributes;
 
namespace ShadowViewer.Plugin.Bika.Enums;

/// <summary>
/// <see cref="ShadowViewer.Plugin.Bika.Settings.BikaSettings"/>
/// </summary>
[ShadowSettingClass(ClassName = "BikaSettings", Container = "ShadowViewer.Plugin.Bika")]
public enum BikaConfigKey
{
    /// <summary>
    /// Api分流
    /// </summary>
    [ShadowSetting(typeof(int), defaultVal: "3", comment: "Api分流")]
    ApiShunt,

    /// <summary>
    /// 图片分流
    /// </summary>
    [ShadowSetting(typeof(int), defaultVal: "3", comment: "图片分流")]
    PicShunt,

    /// <summary>
    /// 登录记住我
    /// </summary>
    [ShadowSetting(typeof(bool),  comment: "登录记住我")]
    RememberMe,

    /// <summary>
    /// 自动登录
    /// </summary>
    [ShadowSetting(typeof(bool), comment: "自动登录")]
    AutoLogin,

    /// <summary>
    /// 最后登录用户
    /// </summary>
    [ShadowSetting(typeof(string), comment: "最后登录用户")]
    LastBikaUser,
    /// <summary>
    /// 代理
    /// </summary>
    [ShadowSetting(typeof(string), comment: "代理")]
    Proxy,
    /// <summary>
    /// 被封印的漫画可以临时解封
    /// </summary>
    [ShadowSetting(typeof(bool), defaultVal: "false", comment: "被封印的漫画可以临时解封")]
    CanTemporaryUnlockComic,
    /// <summary>
    /// 被封印的漫画直接忽略
    /// </summary>
    [ShadowSetting(typeof(bool), defaultVal: "false", comment: "被封印的漫画直接忽略")]
    IsIgnoreLockComic,
    /// <summary>
    /// 被封印的漫画加载
    /// </summary>
    [ShadowSetting(typeof(bool), defaultVal: "false", comment: "被封印的漫画加载")]
    LoadLockComic,
}