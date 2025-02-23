﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class MC : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(MagentaFragment), typeof(CyanFragment)];

    public override void BeforePartHit(State state, Combat combat, Part part, int incomingDamage)
    {
        base.BeforePartHit(state, combat, part, incomingDamage);
        if ((playerOwned ? state.ship : combat.otherShip).Get(Status.tempShield) <= 0) return;
        combat.QueueImmediate(new AStatus
        {
            status = ModEntry.Instance.KokoroApi.OxidationStatus.Status,
            statusAmount = 4,
            timer = 0,
            targetPlayer = !playerOwned
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [.. StatusMeta.GetTooltips(Status.tempShield, 1), .. StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.OxidationStatus.Status, 4)];
}
