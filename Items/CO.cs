using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class CO : Item
{
    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        (playerOwned ? combat.otherShip : state.ship).DirectHullDamage(state, combat, 1);
    }
}