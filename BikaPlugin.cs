using Windows.Storage;
using Microsoft.Extensions.DependencyInjection;
using ShadowViewer.Plugin.Bika.Enums;
using PicaComic;
using Serilog;
using ShadowViewer.Interfaces;

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
        public BikaPlugin()
        {
            Caller = DIFactory.Current.Services.GetService<ICallableToolKit>();
            if (ConfigHelper.Contains(MetaData.Id))
            {
                IsEnabled = ConfigHelper.GetBoolean(MetaData.Id);
            }
            else
            {
                ConfigHelper.Set(MetaData.Id, true);
            }
            if (!ConfigHelper.Contains(BikaSettingName.ApiShunt.ToString()))
            {
                ConfigHelper.Set(BikaSettingName.ApiShunt.ToString(), 3);
            }
            if (!ConfigHelper.Contains(BikaSettingName.PicShunt.ToString()))
            {
                ConfigHelper.Set(BikaSettingName.PicShunt.ToString(), 3);
            }
            PicaClient.AppChannel = ConfigHelper.GetInt32(BikaSettingName.ApiShunt.ToString());
            PicaClient.FileChannel = ConfigHelper.GetInt32(BikaSettingName.PicShunt.ToString());
            
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
            page = typeof(BikaHomePage);
            parameter = null;
        }

    }
}
