using HarmonyLib;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace SupermarketSimulatorMods.BackToWork
{
    /// <summary>
    /// Harmony patch to intercept and modify restocker waiting behavior when storage racks are full.
    /// This is the main patch that enables the "drop box and continue working" functionality.
    /// </summary>
    [HarmonyPatch(typeof(Restocker), "GoToWaiting")]
    public class RestockerGoToWaitingPatch
    {
        /// <summary>
        /// Intercepts the GoToWaiting method when restockers are about to wait for available rack slots.
        /// Instead of waiting with a box, drops the box to ground and resumes other restocking tasks.
        /// </summary>
        /// <param name="__instance">The restocker instance</param>
        /// <param name="state">The waiting state being entered</param>
        /// <returns>False to skip original method when box is dropped, true to continue original behavior</returns>
        static bool Prefix(Restocker __instance, RestockerState state)
        {
            if (state != RestockerState.WAITING_FOR_AVAILABLE_RACK_SLOT)
            {
                return true;
            }

            if (!BackToWorkConfig.EnableMod)
            {
                return true;
            }

            if (!__instance.CarryingBox)
            {
                return true;
            }

            var boxField = AccessTools.Field(typeof(Restocker), "m_Box");
            var box = boxField?.GetValue(__instance);
            
            if (box == null)
            {
                return true;
            }

            if (BackToWorkConfig.VerboseLogging)
            {
                BackToWorkPlugin.Logger.LogInfo($"Restocker carrying box but no rack space available. Dropping box and continuing work.");
            }

            try
            {
                // Use reflection to safely access private methods and fields
                var dropBoxMethod = AccessTools.Method(typeof(Restocker), "DropBoxToGround");
                if (dropBoxMethod == null)
                {
                    BackToWorkPlugin.Logger.LogError("Could not find DropBoxToGround method");
                    return true;
                }
                dropBoxMethod.Invoke(__instance, null);
                
                var stateField = AccessTools.Field(typeof(Restocker), "m_State");
                if (stateField != null)
                {
                    stateField.SetValue(__instance, RestockerState.IDLE);
                }
                
                var checkTasksField = AccessTools.Field(typeof(Restocker), "m_CheckTasks");
                if (checkTasksField != null)
                {
                    checkTasksField.SetValue(__instance, true);
                }
                
                var tryRestockingMethod = AccessTools.Method(typeof(Restocker), "TryRestocking");
                if (tryRestockingMethod != null)
                {
                    var coroutine = tryRestockingMethod.Invoke(__instance, null) as IEnumerator;
                    if (coroutine != null)
                    {
                        __instance.StartCoroutine(coroutine);
                    }
                }
                
                if (BackToWorkConfig.VerboseLogging)
                {
                    BackToWorkPlugin.Logger.LogInfo("Successfully dropped box and resumed restocking tasks.");
                }
                
                return false;
            }
            catch (System.Exception ex)
            {
                BackToWorkPlugin.Logger.LogError($"Error in RestockerGoToWaitingPatch: {ex.Message}");
                BackToWorkPlugin.Logger.LogError($"Stack trace: {ex.StackTrace}");
                return true;
            }
        }
    }
}