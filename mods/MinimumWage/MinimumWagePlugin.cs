using BepInEx;
using BepInEx.Logging;

namespace SupermarketSimulatorMods.MinimumWage
{
    [BepInPlugin("com.supermarketsim.minimumwage", "Minimum Wage", "1.0.0")]
    public class MinimumWagePlugin : ModBase
    {
        internal static new ManualLogSource Logger;
        protected override string GetModName() => "Minimum Wage";
        protected override string GetModVersion() => "1.0.0";
        
        protected override void Awake()
        {
            Logger = base.Logger;
            base.Awake();
        }
        
        protected override void InitializeConfig()
        {
            StaffWageConfig.Initialize(Config);
        }
        
    }
}