namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaConfigHelper
{
    private const string Container = "Bika";
    public static void Set(BikaConfigKey key, object value)
    {
        Set(key.ToString(), value);
    }
    public static bool Contains(BikaConfigKey key)
    {
        return Contains(key.ToString());
    }
    public static string GetString(BikaConfigKey key)
    {
        return GetString( key.ToString());
    }
    public static bool GetBoolean(BikaConfigKey key)
    {
        return GetBoolean( key.ToString());
    }
    public static int GetInt32(BikaConfigKey key)
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