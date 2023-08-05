namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaSettingsHelper
{
    private const string Container = "Bika";
    public static void Set(BikaSettingName key, object value)
    {
        ConfigHelper.Set(Container, key.ToString(), value);
    }
    public static bool Contains(BikaSettingName key)
    {
        return ConfigHelper.Contains(Container, key.ToString());
    }
    public static string GetString(BikaSettingName key)
    {
        return ConfigHelper.GetString(Container, key.ToString());
    }
    public static bool GetBoolean(BikaSettingName key)
    {
        return ConfigHelper.GetBoolean(Container, key.ToString());
    }
    public static int GetInt32(BikaSettingName key)
    {
        return ConfigHelper.GetInt32(Container, key.ToString());
    }
}