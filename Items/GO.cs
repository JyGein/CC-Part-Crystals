using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class GO : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(GreenFragment), typeof(OrangeFragment)];

    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        base.OnPartHit(state, combat, part, damageDone);
        combat.QueueImmediate([new AStatus
        {
            status = Status.heat,
            statusAmount = 0,
            timer = 0,
            mode = AStatusMode.Set,
            targetPlayer = playerOwned
        },
        new AStatus
        {
            status = Status.serenity,
            statusAmount = 1,
            timer = 0,
            targetPlayer = playerOwned
        }]);
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [.. StatusMeta.GetTooltips(Status.heat, 3), .. StatusMeta.GetTooltips(Status.serenity, 1)];
}
