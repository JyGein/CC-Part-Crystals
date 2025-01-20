using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class RG : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(RedFragment), typeof(GreenFragment)];

    public override void OnPartDamages(State state, Combat combat, Part part, DamageDone damageDone, Ship ship)
    {
        if (damageDone.hitHull)
        {
            ship.DirectHullDamage(state, combat, 2);
        }
    }
}