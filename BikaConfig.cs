using System;
using DryIoc;
using PicaComic;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;

namespace ShadowViewer.Plugin.Bika
{
    public static class BikaConfig
    {
        public static void Init()
        {
            var bikaClient = DiFactory.Services.Resolve<IPicaClient>();
            if (!BikaConfigHelper.Contains(BikaConfigKey.ApiShunt))
            {
                BikaConfigHelper.Set(BikaConfigKey.ApiShunt, 3);
            }
            if (!BikaConfigHelper.Contains(BikaConfigKey.PicShunt))
            {
                BikaConfigHelper.Set(BikaConfigKey.PicShunt, 3);
            }
            if (BikaConfigHelper.Contains(BikaConfigKey.Proxy))
            {
                Proxy = BikaConfigHelper.GetString(BikaConfigKey.Proxy);
                try
                {
                    bikaClient.SetProxy(new Uri(Proxy));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            else
            {
                Proxy = "";
            }
            IPicaClient.AppChannel = BikaConfigHelper.GetInt32(BikaConfigKey.ApiShunt);
            IPicaClient.FileChannel = BikaConfigHelper.GetInt32(BikaConfigKey.PicShunt);
            RememberMe = BikaConfigHelper.Contains(BikaConfigKey.RememberMe) && BikaConfigHelper.GetBoolean(BikaConfigKey.RememberMe);
            AutoLogin = BikaConfigHelper.Contains(BikaConfigKey.AutoLogin) && BikaConfigHelper.GetBoolean(BikaConfigKey.AutoLogin);
            CanTemporaryUnlockComic = BikaConfigHelper.Contains(BikaConfigKey.CanTemporaryUnlockComic) && BikaConfigHelper.GetBoolean(BikaConfigKey.CanTemporaryUnlockComic);
            IsIgnoreLockComic = BikaConfigHelper.Contains(BikaConfigKey.IsIgnoreLockComic) && BikaConfigHelper.GetBoolean(BikaConfigKey.IsIgnoreLockComic);
            LoadLockComic = !BikaConfigHelper.Contains(BikaConfigKey.LoadLockComic) || BikaConfigHelper.GetBoolean(BikaConfigKey.LoadLockComic);
            if (BikaConfigHelper.Contains(BikaConfigKey.LastBikaUser))
            {
                LastBikaUser = BikaConfigHelper.GetString(BikaConfigKey.LastBikaUser);
            }
        }
        /// <summary>
        /// 下次登陆是否记住我
        /// </summary>
        public static bool RememberMe
        {
            get => BikaConfigHelper.GetBoolean(BikaConfigKey.RememberMe);
            set => BikaConfigHelper.Set(BikaConfigKey.RememberMe,value);
        }
        /// <summary>
        /// 是否自动登录
        /// </summary>
        public static bool AutoLogin
        {
            get => BikaConfigHelper.GetBoolean(BikaConfigKey.AutoLogin);
            set => BikaConfigHelper.Set(BikaConfigKey.AutoLogin, value);
        }
        /// <summary>
        /// 最后登录用户
        /// </summary>
        public static string LastBikaUser
        {
            get => BikaConfigHelper.GetString(BikaConfigKey.LastBikaUser);
            set => BikaConfigHelper.Set(BikaConfigKey.LastBikaUser, value);
        }
        /// <summary>
        /// 代理
        /// </summary>
        public static string Proxy
        {
            get => BikaConfigHelper.GetString(BikaConfigKey.Proxy);
            set => BikaConfigHelper.Set(BikaConfigKey.Proxy, value);
        }
        /// <summary>
        /// 被封印的漫画可以临时解封
        /// </summary>
        public static bool CanTemporaryUnlockComic
        {
            get => BikaConfigHelper.GetBoolean(BikaConfigKey.CanTemporaryUnlockComic);
            set => BikaConfigHelper.Set(BikaConfigKey.CanTemporaryUnlockComic, value);
        }
        /// <summary>
        /// 被封印的漫画直接忽略
        /// </summary>
        public static bool IsIgnoreLockComic
        {
            get => BikaConfigHelper.GetBoolean(BikaConfigKey.IsIgnoreLockComic);
            set => BikaConfigHelper.Set(BikaConfigKey.IsIgnoreLockComic, value);
        }
        /// <summary>
        /// 被封印的漫画加载
        /// </summary>
        public static bool LoadLockComic
        {
            get => BikaConfigHelper.GetBoolean(BikaConfigKey.LoadLockComic);
            set => BikaConfigHelper.Set(BikaConfigKey.LoadLockComic, value);
        }
    }
}
