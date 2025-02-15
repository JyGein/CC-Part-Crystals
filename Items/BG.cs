using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class GB : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(BlueFragment), typeof(GreenFragment)];

    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        base.OnPartHit(state, combat, part, damageDone);
        if (damageDone.hitHull)
        {
            combat.QueueImmediate([new AHullMax { amount = 1, timer = 0, targetPlayer = playerOwned }, new AHeal { healAmount = 1, targetPlayer = playerOwned }]);
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [new TTGlossary("action.hullMax", 1)];
}
