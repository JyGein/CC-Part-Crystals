using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
//using PartCrystals.Actions;
//using PartCrystals.Cards;
using PartCrystals.External;
using PartCrystals.dumb_stupid_idiot_strings;
using System.Reflection;
using PartCrystals.Features;
using PartCrystals.Fragments;
using System.Runtime.CompilerServices;

namespace PartCrystals.Features;

internal sealed class PartialStatusManager
{
    public PartialStatusManager()
    {
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.Update)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Combat_Update_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(G), nameof(G.Render)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(G_Render_Postfix))
        );
    }

    private static void G_Render_Postfix(double deltaTime, G __instance)
    {
        ModEntry.Instance.Logger.LogInformation(__instance.state.route.GetType().Name);
        if (__instance.state.routeOverride == null) return;
        ModEntry.Instance.Logger.LogInformation(__instance.state.routeOverride!.GetType().Name);
    }

    private static void Combat_Update_Postfix(G g, Combat __instance)
    {
        Partial_Status_Handler(g, __instance, ModEntry.Instance.QuarterEvade.Status, ModEntry.Instance.HalfEvade.Status);
        Partial_Status_Handler(g, __instance, ModEntry.Instance.QuarterHeal.Status, ModEntry.Instance.HalfHeal.Status);
        Partial_Heal_Handler(g, __instance);
        Partial_Enemy_Heal_Handler(g, __instance);
    }

    private static void Partial_Status_Handler(G g, Combat __instance, Status partialStatus, Status fullStatus)
    {
        int toAdd = g.state.ship.Get(partialStatus) / 2;
        if (toAdd == 0)
            return;

        IEnumerable<CardAction> allActions = __instance.cardActions;
        if (__instance.currentCardAction is not null)
            //Append all card actions to the queue check, might not be needed
            allActions = allActions.Append(__instance.currentCardAction);

        foreach (CardAction action in allActions)
            //This checks if any action in the action queue contains the status to be checked, if so we don't execute this code
            if (action is AStatus statusAction && statusAction.status == partialStatus && statusAction.statusAmount < 0)
                return;

        __instance.QueueImmediate([
                new AStatus() { timer = 0, targetPlayer = true, status = fullStatus, statusAmount = toAdd },
                new AStatus() { targetPlayer = true, status = partialStatus, statusAmount = -toAdd * 2 },
            ]);
    }

    private static void Partial_Heal_Handler(G g, Combat __instance)
    {
        Status partialStatus = ModEntry.Instance.HalfHeal.Status;
        int toAdd = g.state.ship.Get(partialStatus) / 2;
        if (toAdd == 0)
            return;

        IEnumerable<CardAction> allActions = __instance.cardActions;
        if (__instance.currentCardAction is not null)
            //Append all card actions to the queue check, might not be needed
            allActions = allActions.Append(__instance.currentCardAction);

        foreach (CardAction action in allActions)
            //This checks if any action in the action queue contains the status to be checked, if so we don't execute this code
            if (action is AStatus statusAction && statusAction.status == partialStatus && statusAction.statusAmount < 0)
                return;

        __instance.QueueImmediate([
            new AHeal() { timer = 0, targetPlayer = true, healAmount = toAdd },
                new AStatus() { targetPlayer = true, status = partialStatus, statusAmount = -toAdd * 2 },
            ]);
    }

    private static void Partial_Enemy_Heal_Handler(G g, Combat __instance)
    {
        Status partialStatus = ModEntry.Instance.HalfHeal.Status;
        int toAdd = __instance.otherShip.Get(partialStatus) / 2;
        if (toAdd == 0)
            return;

        IEnumerable<CardAction> allActions = __instance.cardActions;
        if (__instance.currentCardAction is not null)
            //Append all card actions to the queue check, might not be needed
            allActions = allActions.Append(__instance.currentCardAction);

        foreach (CardAction action in allActions)
            //This checks if any action in the action queue contains the status to be checked, if so we don't execute this code
            if (action is AStatus statusAction && statusAction.status == partialStatus && statusAction.statusAmount < 0)
                return;

        __instance.QueueImmediate([
            new AHeal() { timer = 0, targetPlayer = false, healAmount = toAdd },
                new AStatus() { targetPlayer = false, status = partialStatus, statusAmount = -toAdd * 2 },
            ]);
    }
}
