using HarmonyLib;
using UnityEngine;
using System;

namespace SupermarketSimulatorMods.MinimumWage
{
    // Main patch for EmployeeManager.DailyWage property
    [HarmonyPatch(typeof(EmployeeManager), "DailyWage", MethodType.Getter)]
    public class EmployeeDailyWagePatch
    {
        static void Postfix(ref PlayerPaymentData __result)
        {
            if (__result == null) return;
            
            float originalAmount = __result.Amount;
            MinimumWagePlugin.Logger?.LogInfo($"Original daily staff cost: ${originalAmount:F2}");
            
            // Apply wage modifier (configurable)
            float modifier = StaffWageConfig.WageMultiplier;
            __result.Amount = originalAmount * modifier;
            
            MinimumWagePlugin.Logger?.LogInfo($"Modified daily staff cost: ${__result.Amount:F2} (multiplier: {modifier})");
        }
    }
    
    // Alternative: Patch MoneyManager.MoneyTransition to intercept staff payments
    [HarmonyPatch(typeof(MoneyManager), "MoneyTransition", new Type[] { typeof(float), typeof(MoneyManager.TransitionType), typeof(bool) })]
    public class MoneyTransitionPatch
    {
        static void Prefix(ref float amount, MoneyManager.TransitionType type, bool updateMoneyText)
        {
            // Only modify staff payments (negative values = deductions)
            if (type == MoneyManager.TransitionType.STAFF && amount < 0)
            {
                float originalAmount = amount;
                MinimumWagePlugin.Logger?.LogInfo($"Intercepting staff payment: ${-originalAmount:F2}");
                
                // Apply wage modifier
                amount = originalAmount * StaffWageConfig.WageMultiplier;
                
                MinimumWagePlugin.Logger?.LogInfo($"Modified staff payment: ${-amount:F2}");
            }
        }
    }
    
    // Individual employee wage patches for fine-grained control
    [HarmonyPatch(typeof(RestockerSO), "DailyWage", MethodType.Getter)]
    public class RestockerWagePatch
    {
        static void Postfix(ref float __result)
        {
            if (!StaffWageConfig.EnableIndividualWageControl) return;
            
            float original = __result;
            __result = StaffWageConfig.RestockerWage;
            
            MinimumWagePlugin.Logger?.LogInfo($"Restocker wage: ${original:F2} → ${__result:F2}");
        }
    }
    
    [HarmonyPatch(typeof(CashierSO), "DailyWage", MethodType.Getter)]
    public class CashierWagePatch
    {
        static void Postfix(ref float __result)
        {
            if (!StaffWageConfig.EnableIndividualWageControl) return;
            
            float original = __result;
            __result = StaffWageConfig.CashierWage;
            
            MinimumWagePlugin.Logger?.LogInfo($"Cashier wage: ${original:F2} → ${__result:F2}");
        }
    }
}