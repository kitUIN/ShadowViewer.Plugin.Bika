namespace ShadowViewer.Plugin.Bika.Helpers
{
    public static class BikaResourcesHelper
    {
        private static readonly ResourceManager resourceManager = new ResourceManager();
        private static readonly string prefix = "ShadowViewer.Plugin.Bika/Resources/";
        public static string GetString(string key) 
        {
            return resourceManager.MainResourceMap.GetValue(prefix + key).ValueAsString;
        }
        public static string GetString(BikaResourceKey key)
        {
            return GetString(key.ToString());
        }
    }
}
