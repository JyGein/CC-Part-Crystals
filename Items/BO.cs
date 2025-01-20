using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class BO : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(BlueFragment), typeof(OrangeFragment)];

    public override void OnTurnStart(State state, Combat combat, Part part)
    {
        combat.Queue(new AStatus
        {
            status = Status.serenity,
            statusAmount = 2,
            targetPlayer = playerOwned,
            timer = 0
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(Status.serenity, 2);
}
