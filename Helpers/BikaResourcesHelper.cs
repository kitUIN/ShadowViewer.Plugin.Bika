using CustomExtensions.WinUI;
using Windows.ApplicationModel.Resources.Core;

namespace ShadowViewer.Plugin.Bika.Helpers
{
    public static class BikaResourcesHelper
    {
        private static readonly ResourceMap resourceManager;
        static BikaResourcesHelper()
        {
            resourceManager = ApplicationExtensionHost.GetResourceMapForAssembly();
        }
        public static string GetString(string key) 
        {
            return resourceManager.GetValue(key).ValueAsString;
        }
        public static string GetString(BikaResourceKey key)
        {
            return GetString(key.ToString());
        }
    }
}
