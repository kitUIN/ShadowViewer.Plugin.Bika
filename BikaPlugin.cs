using Windows.Storage;
using ShadowViewer.Plugin.Bika.Enums;
using PicaComic;

namespace ShadowViewer.Plugin.Bika
{
    public class BikaPlugin : IPlugin
    {
        public static PluginMetaData Meta = new PluginMetaData(
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

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsEnabled { get; private set; } = true;

        public BikaPlugin()
        {
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
        public void Started()
        {
            
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void NavigationViewItemsHandler(NavigationViewItem navItem)
        {
            navItem.MenuItems.Add(new NavigationViewItem
            {
                Content = BikaResourcesHelper.GetString(BikaResourceKey.Title),
                Icon = XamlHelper.CreateImageIcon(MetaData.Logo),
                Tag = MetaData.Id,
            });
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
            page = null;
            parameter = null;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Enabled()
        {
            IsEnabled = true;
            ConfigHelper.Set(MetaData.Id,  IsEnabled);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Disabled()
        {
            IsEnabled = false;
            ConfigHelper.Set(MetaData.Id, IsEnabled);
        }
    }
}
