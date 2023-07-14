namespace ShadowViewer.Plugin.Bika.Extensions
{
    /// <summary>
    /// 多语言本地化[哔咔插件]
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    internal sealed class BikaLocaleExtension : MarkupExtension
    {

        /// <summary>
        /// 键值
        /// </summary>
        public BikaResourceKey Key { get; set; }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            return BikaResourcesHelper.GetString(Key);
        }
    }
}

 