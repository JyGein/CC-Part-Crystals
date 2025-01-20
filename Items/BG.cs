using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class BG : Item
{
    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        if (damageDone.hitHull)
        {
            combat.QueueImmediate(new AHullMax { amount = 1, timer = 0 });
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [new TTGlossary("action.hullMax", 1)];
}
