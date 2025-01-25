using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class RO : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(RedFragment), typeof(OrangeFragment)];

    public override void OnPartDamages(State state, Combat combat, Part part, DamageDone damageDone, Ship ship)
    {
        base.OnPartDamages(state, combat, part, damageDone, ship);
        combat.QueueImmediate(new AStatus
        {
            status = Status.heat,
            statusAmount = playerOwned ? 3 : 1,
            targetPlayer = !playerOwned,
            timer = 0
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(Status.heat, 2);
}
