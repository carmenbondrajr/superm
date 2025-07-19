using BepInEx.Configuration;

namespace SupermarketSimulatorMods.MinimumWage
{
    /// <summary>
    /// Configuration management for the Minimum Wage mod.
    /// Handles all user-configurable settings and provides easy access to their values.
    /// </summary>
    public static class StaffWageConfig
    {
        // Configuration entries
        public static ConfigEntry<float> WageMultiplierConfig;
        public static ConfigEntry<bool> EnableLoggingConfig;
        
        // Easy access properties
        public static float WageMultiplier => WageMultiplierConfig.Value;
        public static bool EnableLogging => EnableLoggingConfig.Value;
        
        /// <summary>
        /// Initializes the configuration system with user-configurable settings.
        /// Creates the configuration entries with appropriate ranges and descriptions.
        /// </summary>
        /// <param name="config">The BepInEx ConfigFile instance</param>
        public static void Initialize(ConfigFile config)
        {
            // Main wage multiplier setting
            WageMultiplierConfig = config.Bind("Settings", 
                "WageMultiplier", 
                0.5f, 
                new ConfigDescription("Multiplier for all staff wages. Examples: 1.0 = normal cost, 0.5 = half cost, 0.0 = free staff",
                    new AcceptableValueRange<float>(0f, 2f),
                    new ConfigurationManagerAttributes { Order = 100, ShowRangeAsPercent = false }));
            
            // Logging toggle for debugging
            EnableLoggingConfig = config.Bind("Settings", 
                "EnableLogging", 
                false, 
                new ConfigDescription("Enable detailed logging of wage modifications (useful for debugging)",
                    null,
                    new ConfigurationManagerAttributes { Order = 90, IsAdvanced = true }));
            
            // Log successful initialization
            MinimumWagePlugin.Logger?.LogInfo($"Minimum Wage mod initialized - Wage Multiplier: {WageMultiplier:F2}, Logging: {EnableLogging}");
        }
    }
}