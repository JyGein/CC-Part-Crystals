using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class BR : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(BlueFragment), typeof(RedFragment)];

    public override void AlterAttackFromPart(State state, Combat combat, Part part, AAttack aAttack)
    {
        base.AlterAttackFromPart(state, combat, part, aAttack);
        aAttack.piercing = true;
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [new TTGlossary("action.attackPiercing")];
}
