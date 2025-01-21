using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class YO : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(YellowFragment), typeof(OrangeFragment)];

    public override void OnTurnStart(State state, Combat combat, Part part)
    {
        base.OnTurnStart(state, combat, part);
        if (!playerOwned) return;
        combat.Queue([new AStatus
        {
            status = Status.evade,
            statusAmount = 2,
            targetPlayer = playerOwned,
            timer = 0
        },
        new AStatus
        {
            status = Status.heat,
            statusAmount = 2,
            targetPlayer = playerOwned,
            timer = 0
        }]);
    }
    public override void OnOtherShipTurnStart(State state, Combat combat, Part part)
    {
        base.OnOtherShipTurnStart(state, combat, part);
        if (playerOwned) return;
        combat.Queue(new AStatus
        {
            status = Status.engineStall,
            statusAmount = 1,
            targetPlayer = !playerOwned,
            timer = 0
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => !playerOwned ? StatusMeta.GetTooltips(Status.libra, 1) : [.. StatusMeta.GetTooltips(Status.evade, 2), .. StatusMeta.GetTooltips(Status.heat, 2)];
}
