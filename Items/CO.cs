using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class CO : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(CyanFragment), typeof(OrangeFragment)];

    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        base.OnPartHit(state, combat, part, damageDone);
        (playerOwned ? combat.otherShip : state.ship).DirectHullDamage(state, combat, 1);
    }
}