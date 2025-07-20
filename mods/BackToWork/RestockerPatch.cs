using HarmonyLib;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace SupermarketSimulatorMods.BackToWork
{
    /// <summary>
    /// Harmony patch to enhance restocker productivity when storage racks are full.
    /// This allows restockers to go to the waiting area, drop boxes, and return to productive work.
    /// </summary>
    [HarmonyPatch(typeof(Restocker), "GoToWaiting")]
    public class RestockerGoToWaitingPatch
    {
        /// <summary>
        /// Executes after the restocker has gone to the waiting area.
        /// If they're waiting for rack space while carrying a box, drops the box and returns to productive work.
        /// </summary>
        /// <param name="__instance">The restocker instance</param>
        /// <param name="state">The waiting state that was entered</param>
        /// <param name="__result">The coroutine returned by the original method</param>
        static void Postfix(Restocker __instance, RestockerState state, ref IEnumerator __result)
        {
            if (state != RestockerState.WAITING_FOR_AVAILABLE_RACK_SLOT)
            {
                return;
            }

            if (!BackToWorkConfig.EnableMod)
            {
                return;
            }

            if (!__instance.CarryingBox)
            {
                return;
            }

            var boxField = AccessTools.Field(typeof(Restocker), "m_Box");
            var box = boxField?.GetValue(__instance);
            
            if (box == null)
            {
                return;
            }

            // Replace the original coroutine with our custom one that drops the box after reaching waiting area
            __result = GoToWaitingAndDropBox(__instance, state, __result);
        }

        /// <summary>
        /// Custom coroutine that executes the original waiting behavior, then drops the box and returns to productive work.
        /// </summary>
        /// <param name="restocker">The restocker instance</param>
        /// <param name="state">The waiting state</param>
        /// <param name="originalCoroutine">The original GoToWaiting coroutine</param>
        /// <returns>Modified coroutine that drops box after reaching waiting area</returns>
        static IEnumerator GoToWaitingAndDropBox(Restocker restocker, RestockerState state, IEnumerator originalCoroutine)
        {
            if (BackToWorkConfig.VerboseLogging)
            {
                BackToWorkPlugin.Logger.LogInfo($"Restocker carrying box but no rack space available. Going to waiting area to drop box and get back to work.");
            }

            // Execute the original waiting behavior (go to waiting area)
            yield return restocker.StartCoroutine(originalCoroutine);

            // Now that we're in the waiting area, drop the box and resume tasks
            bool success = false;
            try
            {
                // Use reflection to safely access private methods and fields
                var dropBoxMethod = AccessTools.Method(typeof(Restocker), "DropBoxToGround");
                if (dropBoxMethod == null)
                {
                    BackToWorkPlugin.Logger.LogError("Could not find DropBoxToGround method");
                    yield break;
                }
                dropBoxMethod.Invoke(restocker, null);
                
                var stateField = AccessTools.Field(typeof(Restocker), "m_State");
                if (stateField != null)
                {
                    stateField.SetValue(restocker, RestockerState.IDLE);
                }
                
                var checkTasksField = AccessTools.Field(typeof(Restocker), "m_CheckTasks");
                if (checkTasksField != null)
                {
                    checkTasksField.SetValue(restocker, true);
                }
                
                success = true;
            }
            catch (System.Exception ex)
            {
                BackToWorkPlugin.Logger.LogError($"Error in GoToWaitingAndDropBox: {ex.Message}");
                BackToWorkPlugin.Logger.LogError($"Stack trace: {ex.StackTrace}");
            }

            if (success)
            {
                var tryRestockingMethod = AccessTools.Method(typeof(Restocker), "TryRestocking");
                if (tryRestockingMethod != null)
                {
                    var coroutine = tryRestockingMethod.Invoke(restocker, null) as IEnumerator;
                    if (coroutine != null)
                    {
                        yield return restocker.StartCoroutine(coroutine);
                    }
                    
                    if (BackToWorkConfig.VerboseLogging)
                    {
                        BackToWorkPlugin.Logger.LogInfo("Successfully dropped box in waiting area and returned to productive work.");
                    }
                }
            }
        }
    }
}