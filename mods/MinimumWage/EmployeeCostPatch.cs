using HarmonyLib;
using System;

namespace SupermarketSimulatorMods.MinimumWage
{
    /// <summary>
    /// Harmony patch to intercept and modify staff payment transactions.
    /// This is the main patch that enables wage control functionality.
    /// </summary>
    [HarmonyPatch(typeof(MoneyManager), "MoneyTransition", new Type[] { typeof(float), typeof(MoneyManager.TransitionType), typeof(bool) })]
    public class StaffPaymentPatch
    {
        /// <summary>
        /// Intercepts staff payments before they're processed and applies the wage multiplier.
        /// Only modifies payments of type STAFF with negative amounts (deductions).
        /// </summary>
        /// <param name="amount">Payment amount (negative for deductions)</param>
        /// <param name="type">Type of transaction</param>
        /// <param name="updateMoneyText">Whether to update UI</param>
        static void Prefix(ref float amount, MoneyManager.TransitionType type, bool updateMoneyText)
        {
            // Only modify staff payments (negative values = deductions)
            if (type == MoneyManager.TransitionType.STAFF && amount < 0)
            {
                float originalAmount = amount;
                
                // Apply the configured wage multiplier
                float modifier = StaffWageConfig.WageMultiplier;
                amount = originalAmount * modifier;
                
                // Log the modification for transparency
                if (StaffWageConfig.EnableLogging)
                {
                    MinimumWagePlugin.Logger?.LogInfo($"Staff payment modified: ${-originalAmount:F2} → ${-amount:F2} (multiplier: {modifier:F2})");
                }
            }
        }
    }
}