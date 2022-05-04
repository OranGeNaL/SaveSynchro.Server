namespace SaveSyncro;

public static class ConfigManager
{
    public static string RootDir { get; private set; }

    public static void Initialize()
    {
        if (Environment.GetEnvironmentVariable("RUNNING_IN_CONT") == "1")
        {
            
        }
        else
        {
            RootDir = "~/Documents/saves";
        }

        Directory.CreateDirectory(RootDir);
    }
}