using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class MO : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(MagentaFragment), typeof(OrangeFragment)];

    public override void OnShipOverheats(State state, Combat combat, Part part)
    {
        base.OnShipOverheats(state, combat, part);
        combat.QueueImmediate(new AStatus
        {
            status = Status.tempShield,
            statusAmount = 5,
            timer = 0,
            targetPlayer = playerOwned
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(Status.tempShield, 5);
}
