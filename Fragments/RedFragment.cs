﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class RedFragment : Fragment
{
    public override void OnTurnEnd(State state, Combat combat, Part part)
    {
        base.OnTurnEnd(state, combat, part);
        if (playerOwned)
        {
            combat.Queue(new AAttack
            {
                damage = Card.GetActualDamage(state, 0, false),
                status = ModEntry.Instance.HalfDamage.Status,
                statusAmount = 1,
                targetPlayer = false,
                fromX = state.ship.parts.FindIndex(p => p == part),
                fast = true,
                multiCannonVolley = true
            });
        }
        else
        {
            combat.QueueImmediate(new AAttack
            {
                damage = Card.GetActualDamage(state, 0, true),
                status = ModEntry.Instance.HalfDamage.Status,
                statusAmount = 1,
                targetPlayer = true,
                fromX = combat.otherShip.parts.FindIndex(p => p == part),
                fast = true
            });
        }
    }
    public override void OnPlayerShipShoots(State state, Combat combat, Part part)
    {
        base.OnPlayerShipShoots(state, combat, part);
        return;
#pragma warning disable CS0162 // Unreachable code detected
        combat.QueueImmediate(new AAttack
        {
            damage = Card.GetActualDamage(state, playerOwned ? 0 : 1, !playerOwned),
            status = playerOwned ? ModEntry.Instance.HalfDamage.Status : null,
            statusAmount = playerOwned ? 1 : default,
            targetPlayer = !playerOwned,
            fromX = state.ship.parts.FindIndex(p => p == part),
            fast = true
        });
#pragma warning restore CS0162 // Unreachable code detected
    }

    public override List<Tooltip>? GetExtraTooltips()
        => false ? null : StatusMeta.GetTooltips(ModEntry.Instance.HalfDamage.Status, 1);
}
