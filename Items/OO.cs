using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class OO : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(OrangeFragment), typeof(OrangeFragment)];

    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        base.OnPartHit(state, combat, part, damageDone);
        (playerOwned ? combat.otherShip : state.ship).NormalDamage(state, combat, 2, null);
    }
}