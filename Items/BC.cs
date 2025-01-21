using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class BC : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(BlueFragment), typeof(CyanFragment)];

    bool prevented = false;
    public override void OnTurnStart(State state, Combat combat, Part part)
    {
        base.OnTurnStart(state, combat, part);
        prevented = false;
    }

    public override void AlterHullDamage(State state, Combat combat, Ship ship, ref int amt)
    {
        base.AlterHullDamage(state, combat, ship, ref amt);
        if (!prevented && amt >= 1 && ship.Get(Status.perfectShield) <= 0)
        {
            amt -= 1;
            prevented = true;
        }
    }
}
