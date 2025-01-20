using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class BC : Item
{
    bool prevented = false;
    public override void OnTurnStart(State state, Combat combat, Part part)
    {
        prevented = false;
    }

    public override void AlterHullDamage(State state, Combat combat, Ship ship, ref int amt)
    {
        if (!prevented && amt >= 1 && ship.Get(Status.perfectShield) <= 0)
        {
            amt -= 1;
            prevented = true;
        }
    }
}
