using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class RR : Item
{
    public override void AlterAttackFromPart(State state, Combat combat, Part part, AAttack aAttack)
    {
        aAttack.damage += 1;
    }
}
