using BepInEx;
using BepInEx.Logging;

namespace SupermarketSimulatorMods.BackToWork
{
    /// <summary>
    /// Back to Work - A Supermarket Simulator mod that fixes restocker box holding issues.
    /// 
    /// Features:
    /// - Prevents restockers from getting stuck holding boxes when storage racks are full
    /// - Makes restockers drop boxes to ground and continue other tasks instead of waiting
    /// - Configurable enable/disable functionality
    /// - Optional verbose logging for debugging
    /// - Improves overall store efficiency by keeping restockers productive
    /// 
    /// Technical Details:
    /// - Uses Harmony patching to intercept Restocker.GoToWaiting() method
    /// - Applies reflection-based approach for safe access to private fields
    /// - Maintains compatibility with game's existing restocker management
    /// 
    /// Author: Created with assistance from Claude AI
    /// Version: 1.0.0
    /// Game Version: Supermarket Simulator (tested on current Steam version)
    /// </summary>
    [BepInPlugin("com.supermarketsim.backtowork", "Back to Work", "1.0.0")]
    [BepInProcess("Supermarket Simulator.exe")]
    public class BackToWorkPlugin : ModBase
    {
        internal static new ManualLogSource Logger;
        
        protected override string GetModName() => "Back to Work";
        protected override string GetModVersion() => "1.0.0";
        
        protected override void Awake()
        {
            Logger = base.Logger;
            base.Awake();
        }
        
        protected override void InitializeConfig()
        {
            BackToWorkConfig.Initialize(Config);
        }
    }
}