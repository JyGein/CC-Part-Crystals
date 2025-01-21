using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class BY : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(BlueFragment), typeof(YellowFragment)];

    public override void OnPartAttacks(State state, Combat combat, Part part)
    {
        base.OnPartAttacks(state, combat, part);
        combat.QueueImmediate([new AStatus
        {
            status = ModEntry.Instance.HalfShield.Status,
            statusAmount = 1,
            targetPlayer = playerOwned,
            timer = 0
        },
        new AStatus
        {
            status = ModEntry.Instance.HalfEvade.Status,
            statusAmount = 1,
            targetPlayer = playerOwned,
            timer = 0
        }]);
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [.. StatusMeta.GetTooltips(ModEntry.Instance.HalfShield.Status, 1), .. StatusMeta.GetTooltips(ModEntry.Instance.HalfEvade.Status, 1)];
}
