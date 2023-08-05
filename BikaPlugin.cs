using Windows.Storage;
using Microsoft.Extensions.DependencyInjection;
using ShadowViewer.Plugin.Bika.Enums;
using PicaComic;
using Serilog;
using ShadowViewer.Interfaces;
using SqlSugar;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Bika.Controls;

namespace ShadowViewer.Plugin.Bika
{
    public class BikaPlugin : IPlugin
    {
        private static readonly ILogger Logger = Log.ForContext<BikaPlugin>();
        public static readonly PluginMetaData Meta = new PluginMetaData(
            "Bika",
            "ßÙßÇÂþ»­²å¼þ",
            "ßÙßÇÂþ»­ÊÊÅäÆ÷",
            "kitUIN", "0.1.0",
            new Uri("https://github.com/kitUIN/ShadowViewer.Plugin.Bika/"),
            new Uri("ms-appx:///ShadowViewer.Plugin.Bika/Assets/Icons/logo.png"),
            1);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public PluginMetaData MetaData { get; } = Meta;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public LocalTag AffiliationTag { get; } = new LocalTag(BikaResourcesHelper.GetString(BikaResourceKey.Tag), "#000000", "#ef97b9");
        
        private bool isEnabled = true;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                ConfigHelper.Set(MetaData.Id, value);
                if (IsEnabled)
                {
                    Caller.PluginEnabled(this,MetaData.Id,IsEnabled);
                }
                else
                {
                    Caller.PluginDisabled(this,MetaData.Id,IsEnabled);
                }
            } 
        } 
        private ICallableToolKit Caller { get; }
        private ISqlSugarClient Db { get; }
        public BikaPlugin()
        {
            Caller = DiFactory.Current.Services.GetService<ICallableToolKit>();
            Db = DiFactory.Current.Services.GetService<ISqlSugarClient>();
            Db.CodeFirst.InitTables<BikaUser>();
            if (ConfigHelper.Contains(MetaData.Id))
            {
                IsEnabled = ConfigHelper.GetBoolean(MetaData.Id);
            }
            else
            {
                ConfigHelper.Set(MetaData.Id, true);
            }
            if (!BikaSettingsHelper.Contains(BikaSettingName.ApiShunt))
            {
                BikaSettingsHelper.Set(BikaSettingName.ApiShunt, 3);
            }
            if (!BikaSettingsHelper.Contains(BikaSettingName.PicShunt))
            {
                BikaSettingsHelper.Set(BikaSettingName.PicShunt, 3);
            }
            PicaClient.AppChannel = BikaSettingsHelper.GetInt32(BikaSettingName.ApiShunt);
            PicaClient.FileChannel = BikaSettingsHelper.GetInt32(BikaSettingName.PicShunt);
            var b = false;
            if (BikaSettingsHelper.GetBoolean(BikaSettingName.RememberMe))
            {
                b = TryAutoLogin();
            }
            if (!b)
            {
                var tip = new LoginTip();
                Caller.TopGrid(this, tip,  ShadowViewer.Enums.TopGridMode.Dialog);
                tip.Open();
            }
        }
        /// <summary>
        /// ×Ô¶¯µÇÂ¼
        /// </summary>
        private bool TryAutoLogin()
        {
            var user = BikaSettingsHelper.GetString(BikaSettingName.LastBikaUser);
            if (Db.Queryable<BikaUser>().First(x => x.Email == user) is BikaUser bikaUser)
            {
                PicaClient.SetToken(bikaUser.Token);
                return true;
            }

            return false;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void NavigationViewItemsHandler(ref NavigationViewItem navItem)
        {
            navItem.MenuItems.Add(new NavigationViewItem
            {
                Content = BikaResourcesHelper.GetString(BikaResourceKey.Title),
                Icon = XamlHelper.CreateImageIcon(MetaData.Logo),
                Tag = MetaData.Id,
            });
            Logger.Information("[{Name}]²å¼þµ¼º½À¸×¢Èë³É¹¦",MetaData.Name);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Type SettingsPage => typeof(BikaSettingsPage);


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void NavigationViewItemInvokedHandler(string tag, out Type page, out object parameter)
        {
            page = typeof(ClassificationPage);
            parameter = null;
        }

    }
}
