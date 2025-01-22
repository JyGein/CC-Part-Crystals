﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class RedFragment : Fragment
{
    public override void OnTurnEnd(State state, Combat combat, Part part)
    {
        if (playerOwned) return;
        combat.Queue(new AAttack
        {
            damage = Card.GetActualDamage(state, playerOwned ? 0 : 1, !playerOwned),
            status = playerOwned ? ModEntry.Instance.HalfDamage.Status : null,
            statusAmount = playerOwned ? 1 : default,
            targetPlayer = !playerOwned,
            fromX = combat.otherShip.parts.FindIndex(p => p == part),
            fast = true
        });
    }
    public override void OnPlayerShipShoots(State state, Combat combat, Part part)
    {
        combat.QueueImmediate(new AAttack
        {
            damage = Card.GetActualDamage(state, playerOwned ? 0 : 1, !playerOwned),
            status = playerOwned ? ModEntry.Instance.HalfDamage.Status : null,
            statusAmount = playerOwned ? 1 : default,
            targetPlayer = !playerOwned,
            fromX = state.ship.parts.FindIndex(p => p == part),
            fast = true
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => !playerOwned ? null : StatusMeta.GetTooltips(ModEntry.Instance.HalfDamage.Status, 1);
}
