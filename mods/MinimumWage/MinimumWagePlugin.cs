using BepInEx;
using BepInEx.Logging;

namespace SupermarketSimulatorMods.MinimumWage
{
    /// <summary>
    /// Minimum Wage - A Supermarket Simulator mod that allows you to control staff wages.
    /// 
    /// Features:
    /// - Adjust staff wage costs with a simple multiplier (0.0 = free, 0.5 = half cost, 1.0 = normal)
    /// - Real-time configuration via Configuration Manager (F1) or config file
    /// - Optional detailed logging for debugging
    /// - Compatible with BepInEx Configuration Manager for in-game settings
    /// 
    /// Author: Created with assistance from Claude AI
    /// Version: 1.0.0
    /// Game Version: Supermarket Simulator (tested on current Steam version)
    /// </summary>
    [BepInPlugin("com.supermarketsim.minimumwage", "Minimum Wage", "1.0.0")]
    [BepInProcess("Supermarket Simulator.exe")]
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