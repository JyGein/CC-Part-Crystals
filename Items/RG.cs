using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class RG : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(RedFragment), typeof(GreenFragment)];

    public override void OnPartDamages(State state, Combat combat, Part part, DamageDone damageDone, Ship ship)
    {
        base.OnPartDamages(state, combat, part, damageDone, ship);
        if (damageDone.hitHull && (playerOwned ? combat.otherShip : state.ship).Get(Status.perfectShield) <= 0)
        {
            ship.DirectHullDamage(state, combat, 2);
        }
    }
}