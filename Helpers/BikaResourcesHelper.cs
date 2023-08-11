using CustomExtensions.WinUI;
using Windows.ApplicationModel.Resources.Core;
using ShadowViewer.Plugin.Bika.Enums;

namespace ShadowViewer.Plugin.Bika.Helpers
{
    public static class BikaResourcesHelper
    {
        private static readonly ResourceMap ResourceManager;
        static BikaResourcesHelper()
        {
            ResourceManager = ApplicationExtensionHost.GetResourceMapForAssembly();
        }
        public static string GetString(string key) 
        {
            return ResourceManager.GetValue(key).ValueAsString;
        }
        public static string GetString(BikaResourceKey key)
        {
            return GetString(key.ToString());
        }
    }
}
