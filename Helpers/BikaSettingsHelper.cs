namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaSettingsHelper
{
    private const string Container = "Bika";
    public static void Set(BikaSettingName key, object value)
    {
        Set(key.ToString(), value);
    }
    public static bool Contains(BikaSettingName key)
    {
        return Contains(key.ToString());
    }
    public static string GetString(BikaSettingName key)
    {
        return GetString( key.ToString());
    }
    public static bool GetBoolean(BikaSettingName key)
    {
        return GetBoolean( key.ToString());
    }
    public static int GetInt32(BikaSettingName key)
    {
        return GetInt32( key.ToString());
    }


    public static void Set(string key, object value)
    {
        ConfigHelper.Set(Container, key, value);
    }
    public static bool Contains(string key)
    {
        return ConfigHelper.Contains(Container, key);
    }
    public static string GetString(string key)
    {
        return ConfigHelper.GetString(Container, key);
    }
    public static bool GetBoolean(string key)
    {
        return ConfigHelper.GetBoolean(Container, key);
    }
    public static int GetInt32(string key)
    {
        return ConfigHelper.GetInt32(Container, key);
    }
}