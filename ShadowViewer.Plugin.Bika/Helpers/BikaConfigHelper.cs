using System;
using ShadowViewer.Helpers;
using ShadowViewer.Plugin.Bika.Enums;

namespace ShadowViewer.Plugin.Bika.Helpers;

public class BikaConfigHelper
{
    const string Container = "Bika";
    public static void Set(BikaConfigKey key, object value)
    {
        ConfigHelper.Set(key.ToString(), value, Container);
    }
    public static bool Contains(BikaConfigKey key)
    {
        return ConfigHelper.Contains(key.ToString(), Container);
    }
    public static string GetString(BikaConfigKey key)
    {
        return ConfigHelper.GetString(key.ToString(), Container);
    }
    public static bool GetBoolean(BikaConfigKey key)
    {
        return ConfigHelper.GetBoolean(key.ToString(), Container);
    }
    public static int GetInt32(BikaConfigKey key)
    {
        return ConfigHelper.GetInt32(key.ToString(), Container);
    }
    public static long GetInt64(BikaConfigKey key)
    {
        return ConfigHelper.GetInt64(key.ToString(), Container);
    }
    public static double GetDouble(BikaConfigKey key)
    {
        return ConfigHelper.GetDouble(key.ToString(), Container);
    }
    public static float GetFloat(BikaConfigKey key)
    {
        return ConfigHelper.GetFloat(key.ToString(), Container);
    }
    public static DateTime GetDateTime(BikaConfigKey key)
    {
        return ConfigHelper.GetDateTime(key.ToString(), Container);
    }

}