using Microsoft.EntityFrameworkCore;
using ShadowViewer.Plugin.Local.Database;
using ShadowViewer.Sdk.Enums;
using ShadowViewer.Sdk.Models.Interfaces;

// Only host services and generated dependency injection are adapted; responders and EF mappings are production code.
namespace ShadowPluginLoader.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AutowiredAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EntryPointAttribute : Attribute { public string Name { get; set; } = ""; }
}
namespace ShadowViewer.Sdk.Plugins
{
    public static class PluginResponder { public const string HistoryResponder = "HistoryResponder"; }
}
namespace ShadowViewer.Sdk.Responders
{
    public abstract class AbstractHistoryResponder
    {
        public abstract IEnumerable<IHistory> GetHistories(HistoryMode mode = HistoryMode.Day);
        public abstract void ClickHistoryHandler(IHistory history);
        public abstract void DeleteHistoryHandler(IHistory history);
    }
}
namespace ShadowViewer.Sdk.Services
{
    public interface INavigateService { void Navigate(Type page, object parameter, bool force = false); }
}
namespace ShadowViewer.Plugin.Bika.Pages { public class BikaInfoPage { } }
namespace ShadowViewer.Plugin.Local.Pages { public class AttributesPage { } }
namespace ShadowViewer.Plugin.Bika.Responders
{
    public partial class BikaHistoryResponder
    {
        public BikaHistoryResponder(IDbContextFactory<LocalDbContext> factory)
        {
            DbFactory = factory;
            NavigateService = null!;
        }
    }
}
namespace ShadowViewer.Plugin.Local.Responders
{
    public partial class LocalHistoryResponder
    {
        public LocalHistoryResponder(IDbContextFactory<LocalDbContext> factory)
        {
            DbFactory = factory;
            NavigateService = null!;
        }
    }
}
