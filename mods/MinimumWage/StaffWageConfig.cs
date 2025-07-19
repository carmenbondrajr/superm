using BepInEx.Configuration;

namespace SupermarketSimulatorMods.MinimumWage
{
    public static class StaffWageConfig
    {
        // Configuration entries
        public static ConfigEntry<float> WageMultiplierConfig;
        public static ConfigEntry<bool> EnableIndividualWageControlConfig;
        public static ConfigEntry<float> RestockerWageConfig;
        public static ConfigEntry<float> CashierWageConfig;
        public static ConfigEntry<float> CustomerHelperWageConfig;
        public static ConfigEntry<float> SecurityGuardWageConfig;
        public static ConfigEntry<float> JanitorWageConfig;
        
        // Easy access properties
        public static float WageMultiplier => WageMultiplierConfig.Value;
        public static bool EnableIndividualWageControl => EnableIndividualWageControlConfig.Value;
        public static float RestockerWage => RestockerWageConfig.Value;
        public static float CashierWage => CashierWageConfig.Value;
        public static float CustomerHelperWage => CustomerHelperWageConfig.Value;
        public static float SecurityGuardWage => SecurityGuardWageConfig.Value;
        public static float JanitorWage => JanitorWageConfig.Value;
        
        public static void Initialize(ConfigFile config)
        {
            // Global wage multiplier (affects all employees)
            WageMultiplierConfig = config.Bind("General", 
                "WageMultiplier", 
                0.5f, 
                new ConfigDescription("Multiplier for all staff wages (1.0 = normal, 0.5 = half cost, 0.0 = free)",
                    new AcceptableValueRange<float>(0f, 3f),
                    new ConfigurationManagerAttributes { Order = 100, ShowRangeAsPercent = false }));
            
            // Individual wage control toggle
            EnableIndividualWageControlConfig = config.Bind("General", 
                "EnableIndividualWageControl", 
                false, 
                new ConfigDescription("Enable individual wage settings per employee type (overrides multiplier)",
                    null,
                    new ConfigurationManagerAttributes { Order = 90 }));
            
            // Individual wage settings
            RestockerWageConfig = config.Bind("Individual Wages", 
                "RestockerWage", 
                25.0f, 
                new ConfigDescription("Daily wage for restockers (only used if EnableIndividualWageControl is true)",
                    new AcceptableValueRange<float>(0f, 500f),
                    new ConfigurationManagerAttributes { Order = 50, IsAdvanced = false }));
            
            CashierWageConfig = config.Bind("Individual Wages", 
                "CashierWage", 
                30.0f, 
                new ConfigDescription("Daily wage for cashiers (only used if EnableIndividualWageControl is true)",
                    new AcceptableValueRange<float>(0f, 500f),
                    new ConfigurationManagerAttributes { Order = 40, IsAdvanced = false }));
            
            CustomerHelperWageConfig = config.Bind("Individual Wages", 
                "CustomerHelperWage", 
                20.0f, 
                new ConfigDescription("Daily wage for customer helpers (only used if EnableIndividualWageControl is true)",
                    new AcceptableValueRange<float>(0f, 500f),
                    new ConfigurationManagerAttributes { Order = 30, IsAdvanced = false }));
            
            SecurityGuardWageConfig = config.Bind("Individual Wages", 
                "SecurityGuardWage", 
                35.0f, 
                new ConfigDescription("Daily wage for security guards (only used if EnableIndividualWageControl is true)",
                    new AcceptableValueRange<float>(0f, 500f),
                    new ConfigurationManagerAttributes { Order = 20, IsAdvanced = false }));
            
            JanitorWageConfig = config.Bind("Individual Wages", 
                "JanitorWage", 
                20.0f, 
                new ConfigDescription("Daily wage for janitors (only used if EnableIndividualWageControl is true)",
                    new AcceptableValueRange<float>(0f, 500f),
                    new ConfigurationManagerAttributes { Order = 10, IsAdvanced = false }));
            
            MinimumWagePlugin.Logger?.LogInfo("Staff wage configuration initialized:");
            MinimumWagePlugin.Logger?.LogInfo($"  Wage Multiplier: {WageMultiplier}");
            MinimumWagePlugin.Logger?.LogInfo($"  Individual Control: {EnableIndividualWageControl}");
            
            if (EnableIndividualWageControl)
            {
                MinimumWagePlugin.Logger?.LogInfo($"  Restocker: ${RestockerWage:F2}");
                MinimumWagePlugin.Logger?.LogInfo($"  Cashier: ${CashierWage:F2}");
                MinimumWagePlugin.Logger?.LogInfo($"  Customer Helper: ${CustomerHelperWage:F2}");
                MinimumWagePlugin.Logger?.LogInfo($"  Security Guard: ${SecurityGuardWage:F2}");
                MinimumWagePlugin.Logger?.LogInfo($"  Janitor: ${JanitorWage:F2}");
            }
        }
    }
}