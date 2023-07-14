using Microsoft.Extensions.DependencyInjection;
using ShadowViewer.Extensions;
using ShadowViewer.Interfaces;
using ShadowViewer.ToolKits;
using ShadowViewer.ViewModels;

namespace ShadowViewer.Plugin.Bika
{
    public class BikaPlugin : IPlugin
    {
        private IResourcesToolKit resourcesToolKit;
        
        private readonly PluginMetaData metaData = new PluginMetaData(
            "Bika",
            "ßÙßÇÂþ»­",
                "ßÙßÇÂþ»­ÊÊÅäÆ÷",
                "kitUIN", "0.1.0",
                new Uri("https://github.com/kitUIN/ShadowViewer/tree/master/ShadowViewer.Plguin.Bika/README.md"),
                new Uri("ms-appx://ShadowViewer.Plguin.Bika/Assets/Icons/logo.png"),
                1);
        private readonly LocalTag affiliationTag;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public PluginMetaData MetaData { get => metaData; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public LocalTag AffiliationTag { get => affiliationTag; }
        public BikaPlugin(IEnumerable<IResourcesToolKit> resourcesToolKits)
        {
            resourcesToolKit = resourcesToolKits.First(x => x is BikaResourcesToolKit);
            affiliationTag = new LocalTag(resourcesToolKit.GetString("Bika.Tag.Bika"), "#000000", "#ef97b9");
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
                Content = resourcesToolKit.GetString("Bika.NavigationItem.Title"),
                Icon = XamlHelper.CreateImageIcon(MetaData.Logo),
                Tag = MetaData.ID,
            });
        }

        public Type NavigationPage()
        {
            return typeof(BikaHomePage);
        }

        public Type NavigationViewItemInvokedHandler(string tag)
        {
            if(tag == MetaData.ID)
                return typeof(BikaHomePage);
            return null;
        }

        public void PluginSettingsExpander(SettingsExpander expander)
        {
            SettingsCard webUri = new SettingsCard
            {
                Header = resourcesToolKit.GetString("Bika.WebUriSettingsCard.Title"),
                HeaderIcon = XamlHelper.CreateBitmapIcon("ms-appx://ShadowViewer.Plguin.Bika/Assets/Icons/github.png"),
                Description = "GitHub@" + MetaData.Author,
                IsClickEnabled = true,
                ActionIcon = XamlHelper.CreateFontIcon("\uE8A7"),
                Tag = true,
            };
            webUri.Click += (s, e) =>
            {
                MetaData.WebUri.LaunchUriAsync();
                
            };
            expander.Items.Add(webUri);

        }

        public void NavigationViewItemInvokedHandler(string tag, out Type _page, out object parameter)
        {
            _page = null;
            parameter = null;
        }
    }
}
