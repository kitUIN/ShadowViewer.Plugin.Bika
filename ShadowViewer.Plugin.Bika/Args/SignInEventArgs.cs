using System;

namespace ShadowViewer.Plugin.Bika.Args;

/// <summary>
/// 登录成功事件参数
/// </summary>
public class SignInEventArgs : EventArgs
{
    /// <summary>
    /// 登录的邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 登录Token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 登录成功事件参数
    /// </summary>
    public SignInEventArgs(string email, string token)
    {
        Email = email;
        Token = token;
    }
}