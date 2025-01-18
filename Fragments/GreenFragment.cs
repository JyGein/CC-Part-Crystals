﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class GreenFragment : Fragment
{
    public override void OnPartHit(State state, Combat combat)
    {
        combat.QueueImmediate(playerOwned ?
            new AStatus
            {
                status = ModEntry.Instance.QuarterHeal.Status,
                statusAmount = 1,
                targetPlayer = true
            } :
            new AHeal
            {
                healAmount = 1,
                targetPlayer = false
            });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => !playerOwned ? null : StatusMeta.GetTooltips(ModEntry.Instance.QuarterHeal.Status, 1);
}
