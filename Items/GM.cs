using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class GM : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(GreenFragment), typeof(MagentaFragment)];

    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        base.OnPartHit(state, combat, part, damageDone);
        combat.QueueImmediate([new AStatus
        {
            status = ModEntry.Instance.QuarterHeal.Status,
            statusAmount = 1,
            timer = 0,
            targetPlayer = playerOwned
        },
        new AStatus
        {
            status = Status.tempShield,
            statusAmount = 3,
            timer = 0,
            targetPlayer = playerOwned
        }]);
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [.. StatusMeta.GetTooltips(ModEntry.Instance.QuarterHeal.Status, 1), .. StatusMeta.GetTooltips(Status.tempShield, 3)];
}
