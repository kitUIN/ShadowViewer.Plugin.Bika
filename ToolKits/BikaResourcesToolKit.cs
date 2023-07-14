using ShadowViewer.Interfaces;
using System.Diagnostics;

namespace ShadowViewer.ToolKits
{
    public class BikaResourcesToolKit: IResourcesToolKit
    {
        private readonly ResourceManager resourceManager = new ResourceManager();
        private readonly string prefix = "ShadowViewer.Plugin.Bika/Resources/";
        public BikaResourcesToolKit() { }
        public string GetString(string key)
        {
            return resourceManager.MainResourceMap.GetValue(this.prefix + key.Replace(".", "/")).ValueAsString;
        }
    }
}
