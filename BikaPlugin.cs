using Microsoft.Extensions.DependencyInjection;
using ShadowViewer.Extensions;
using ShadowViewer.Interfaces;
using ShadowViewer.ToolKits;
using ShadowViewer.ViewModels;

namespace ShadowViewer.Plugin.Bika
{
    public class BikaPlugin : IPlugin
    {
        private bool isEnabled = true;
        private static  readonly  PluginMetaData metaData = new PluginMetaData(
            "Bika",
            "ßÙßÇÂþ»­",
                "ßÙßÇÂþ»­ÊÊÅäÆ÷",
                "kitUIN", "0.1.0",
                new Uri("https://github.com/kitUIN/ShadowViewer.Plugin.Bika/"),
                new Uri("ms-appx://ShadowViewer.Plugin.Bika/Assets/Icons/logo.png"),
                1);
        private static readonly LocalTag affiliationTag = new LocalTag(BikaResourcesHelper.GetString(BikaResourceKey.Tag), "#000000", "#ef97b9");
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public PluginMetaData MetaData { get => metaData; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public LocalTag AffiliationTag { get => affiliationTag; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsEnabled => isEnabled;
        
        public BikaPlugin()
        {
            if (ConfigHelper.Contains(metaData.Id))
            {
                isEnabled = ConfigHelper.GetBoolean(metaData.Id);
            }
            else
            {
                ConfigHelper.Set(metaData.Id, true);
            }
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
        // <summary>
        /// <inheritdoc/>
        /// </summary>
        public Type SettingsPage => typeof(BikaSettingsPage);


        // <summary>
        /// <inheritdoc/>
        /// </summary>
        public void NavigationViewItemInvokedHandler(string tag, out Type _page, out object parameter)
        {
            _page = null;
            parameter = null;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Enabled()
        {
            isEnabled = true;
            ConfigHelper.Set(metaData.Id,  isEnabled);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Disabled()
        {
            isEnabled = false;
            ConfigHelper.Set(metaData.Id, isEnabled);
        }
    }
}
