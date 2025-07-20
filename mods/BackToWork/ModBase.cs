using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SupermarketSimulatorMods.BackToWork
{
    /// <summary>
    /// Base class for Back to Work mod components.
    /// Provides common functionality for mod initialization, configuration, and Harmony patching.
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
        
        protected abstract string GetModName();
        protected abstract string GetModVersion();
        protected virtual void InitializeConfig() { }
        
        protected virtual void ApplyPatches()
        {
            HarmonyInstance = Harmony.CreateAndPatchAll(GetType().Assembly);
        }
    }
}