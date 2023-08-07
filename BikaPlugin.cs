using System.Collections.ObjectModel;
using Windows.Storage;
using Microsoft.Extensions.DependencyInjection;
using ShadowViewer.Plugin.Bika.Enums;
using PicaComic;
using Serilog;
using ShadowViewer.Controls;
using ShadowViewer.Enums;
using ShadowViewer.Interfaces;
using SqlSugar;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Bika.Controls;

namespace ShadowViewer.Plugin.Bika
{
    public class BikaPlugin : IPlugin
    {
        private static readonly ILogger Logger = Log.ForContext<BikaPlugin>();
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public PluginMetaData MetaData { get; } = Meta;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public LocalTag AffiliationTag { get; } = new LocalTag(BikaResourcesHelper.GetString(BikaResourceKey.Tag), "#000000", "#ef97b9");
        
        private bool isEnabled = false;
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

        public static readonly PluginMetaData Meta = new PluginMetaData(
            "Bika",
            "ßÙßÇÂþ»­",
            "ßÙßÇÂþ»­ÊÊÅäÆ÷",
            "kitUIN", "0.1.0",
            new Uri("https://github.com/kitUIN/ShadowViewer.Plugin.Bika/"),
            new Uri("ms-appx:///ShadowViewer.Plugin.Bika/Assets/Icons/logo.png"),
            1);
        
        public BikaPlugin()
        {
            BikaData.Current = new BikaData();
            BikaConfig.Init();
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
            Caller.PluginEnabledEvent += PluginEnabled;
        } 

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void NavigationViewMenuItemsHandler(ObservableCollection<NavigationViewItem> menus)
        {
            var root =new NavigationViewItem
            {
                Content = BikaResourcesHelper.GetString(BikaResourceKey.Title),
                Icon = XamlHelper.CreateImageIcon(MetaData.Logo),
                Tag = MetaData.Id,
            };
            /*root.MenuItems.Add(new NavigationViewItem
            {
                Content = BikaResourcesHelper.GetString(BikaResourceKey.Home),
                Icon = new SymbolIcon(Symbol.Home),
                Tag = NavigationViewTag.BikaHome.ToString(),
                 
            });
            root.MenuItems.Add(new NavigationViewItem
            {
                Content = BikaResourcesHelper.GetString(BikaResourceKey.Classification),
                Icon = new SymbolIcon(Symbol.AllApps),
                Tag = NavigationViewTag.BikaClassification.ToString(),
            });*/
            if (!menus.Any(x=>x.Tag is string tag && tag == MetaData.Id))
            {
                menus.Add(root);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void NavigationViewFooterItemsHandler(ObservableCollection<NavigationViewItem> menus)
        {
            
        }

        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async void Loaded()
        {
            // Î´¼ÓÔØ·âÓ¡Ôò¼ÓÔØ
            if (BikaData.Current.Locks.Count == 0)
            {
                foreach (var item in BikaData.Categories)
                {
                    if (ConfigHelper.Contains(item))
                    {
                        BikaData.Current.Locks.Add(
                            new BikaLock(item,
                            ConfigHelper.GetBoolean(item))
                            );
                    }
                    else
                    {
                        ConfigHelper.Set(item, true);
                    }
                }
            }
            // Î´¼ÓÔØµÇÂ¼Æ¾Ö¤Ôò¼ÓÔØ
            if (!PicaClient.HasToken)
            {
                var b = false;
                if (BikaConfig.AutoLogin)
                {
                    b = TryAutoLogin();
                }
                if (!b)
                {
                    var tip = new LoginTip();
                    Caller.TopGrid(this, tip, TopGridMode.Dialog);
                    tip.Open();
                }
                else
                {
                    await BikaHttpHelper.Profile(this);
                    await BikaHttpHelper.PunchIn(this);
                    await BikaHttpHelper.Keywords();
                }
            }
        }
        /// <summary>
        /// ×Ô¶¯µÇÂ¼
        /// </summary>
        private bool TryAutoLogin()
        {
            var user = BikaConfig.LastBikaUser;
            if (Db.Queryable<BikaUser>().First(x => x.Email == user) is BikaUser bikaUser)
            {
                PicaClient.SetToken(bikaUser.Token);
                Caller.TopGrid(this, new TipPopup(
                    $"[{Meta.Name}]{BikaResourcesHelper.GetString(BikaResourceKey.AutoLoginSuccess)}",
                    InfoBarSeverity.Success), TopGridMode.Tip);
                return true;
            }
            return false;
        }
         
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Type SettingsPage => typeof(BikaSettingsPage);


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void NavigationViewItemInvokedHandler(object tag, ref Type page, ref object parameter)
        {
            page = typeof(ClassificationPage);
            parameter = null;
        }

        /// <summary>
        /// ²å¼þÆô¶¯ºó´¥·¢
        /// </summary>
        void PluginEnabled(object sender, ShadowViewer.Args.PluginEventArg e)
        {
            if(e.PluginId == MetaData.Id && isEnabled)
            {
                Loaded();
            }
        }
        /// <summary>
        /// ²å¼þ½ûÓÃºó´¥·¢
        /// </summary>
        void PluginDisabled(object sender, ShadowViewer.Args.PluginEventArg e)
        {

        }
    }
}
