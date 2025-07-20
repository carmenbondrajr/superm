using BepInEx.Configuration;

namespace SupermarketSimulatorMods.BackToWork
{
    /// <summary>
    /// Configuration management for the Back to Work mod.
    /// Handles all user-configurable settings and provides easy access to their values.
    /// </summary>
    public static class BackToWorkConfig
    {
        // Configuration entries
        public static ConfigEntry<bool> EnableModConfig;
        public static ConfigEntry<bool> VerboseLoggingConfig;
        
        // Easy access properties
        public static bool EnableMod => EnableModConfig.Value;
        public static bool VerboseLogging => VerboseLoggingConfig.Value;
        
        /// <summary>
        /// Initializes the configuration system with user-configurable settings.
        /// Creates the configuration entries with appropriate descriptions and UI ordering.
        /// </summary>
        /// <param name="config">The BepInEx ConfigFile instance</param>
        public static void Initialize(ConfigFile config)
        {
            // Main enable/disable toggle
            EnableModConfig = config.Bind("Settings", 
                "EnableMod", 
                true, 
                new ConfigDescription("Enable the Back to Work mod functionality. When enabled, restockers will go to the waiting area, drop boxes there, and return to productive work instead of sitting idle when storage racks are full.",
                    null,
                    new ConfigurationManagerAttributes { Order = 100 }));
                
            // Logging toggle for debugging
            VerboseLoggingConfig = config.Bind("Settings",
                "VerboseLogging",
                false,
                new ConfigDescription("Enable detailed logging of restocker productivity enhancements (useful for debugging)",
                    null,
                    new ConfigurationManagerAttributes { Order = 90, IsAdvanced = true }));
            
            // Log successful initialization
            BackToWorkPlugin.Logger?.LogInfo($"Back to Work mod initialized - Enabled: {EnableMod}, Verbose Logging: {VerboseLogging}");
        }
    }
}