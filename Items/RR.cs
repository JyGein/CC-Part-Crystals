using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class RR : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(RedFragment), typeof(RedFragment)];

    public override void AlterAttackFromPart(State state, Combat combat, Part part, AAttack aAttack)
    {
        base.AlterAttackFromPart(state, combat, part, aAttack);
        aAttack.damage += 1;
    }
}
