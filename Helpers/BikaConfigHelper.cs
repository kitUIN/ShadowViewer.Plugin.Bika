namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaConfigHelper
{
 
    public static void Set(BikaConfigKey key, string value)
    {
        ConfigHelper.Set(key.ToString(), value);
    }
    public static void Set(BikaConfigKey key, int value)
    {
        ConfigHelper.Set(key.ToString(), value);
    }
    public static void Set(BikaConfigKey key, double value)
    {
        ConfigHelper.Set(key.ToString(), value);
    }
    public static void Set(BikaConfigKey key, float value)
    {
        ConfigHelper.Set(key.ToString(), value);
    }
    public static void Set(BikaConfigKey key, bool value)
    {
        ConfigHelper.Set(key.ToString(), value);
    }
    public static void Set(BikaConfigKey key, long value)
    {
        ConfigHelper.Set(key.ToString(), value);
    }
    public static void Set(BikaConfigKey key, DateTime value)
    {
        ConfigHelper.Set(key.ToString(), value);
    }
    public static bool Contains(BikaConfigKey key)
    {
        return ConfigHelper.Contains(key.ToString());
    }
    public static string GetString(BikaConfigKey key)
    {
        return ConfigHelper.GetString(key.ToString());
    }
    public static bool GetBoolean(BikaConfigKey key)
    {
        return ConfigHelper.GetBoolean(key.ToString());
    }
    public static int GetInt32(BikaConfigKey key)
    {
        return ConfigHelper.GetInt32(key.ToString());
    }
    public static long GetInt64(BikaConfigKey key)
    {
        return ConfigHelper.GetInt64(key.ToString());
    }
    public static double GetDouble(BikaConfigKey key)
    {
        return ConfigHelper.GetDouble(key.ToString());
    }
    public static float GetFloat(BikaConfigKey key)
    {
        return ConfigHelper.GetFloat(key.ToString());
    }
    public static DateTime GetDateTime(BikaConfigKey key)
    {
        return ConfigHelper.GetDateTime(key.ToString());
    }

}