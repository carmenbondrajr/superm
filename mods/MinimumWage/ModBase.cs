using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SupermarketSimulatorMods.MinimumWage
{
    /// <summary>
    /// Base class for this mod - provides common functionality
    /// </summary>
    public abstract class ModBase : BaseUnityPlugin
    {
        protected ManualLogSource ModLogger;
        protected Harmony HarmonyInstance;
        
        protected virtual void Awake()
        {
            ModLogger = base.Logger;
            ModLogger.LogInfo($"{GetModName()} v{GetModVersion()} loaded!");
            
            InitializeConfig();
            ApplyPatches();
            
            ModLogger.LogInfo($"{GetModName()} initialization complete!");
        }
        
        protected virtual void OnDestroy()
        {
            HarmonyInstance?.UnpatchSelf();
            ModLogger?.LogInfo($"{GetModName()} unloaded!");
        }
        
        /// <summary>
        /// Override to return your mod's display name
        /// </summary>
        protected abstract string GetModName();
        
        /// <summary>
        /// Override to return your mod's version
        /// </summary>
        protected abstract string GetModVersion();
        
        /// <summary>
        /// Override to initialize mod-specific configuration
        /// </summary>
        protected virtual void InitializeConfig() { }
        
        /// <summary>
        /// Override to apply Harmony patches
        /// </summary>
        protected virtual void ApplyPatches()
        {
            HarmonyInstance = Harmony.CreateAndPatchAll(GetType().Assembly);
        }
    }
}