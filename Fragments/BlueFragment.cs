using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class BlueFragment : Fragment
{
    public override void OnTurnStart(State state, Combat combat, Part part)
    {
        combat.Queue(new AStatus
        {
            status = ModEntry.Instance.HalfShield.Status,
            statusAmount = 1,
            targetPlayer = playerOwned
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(ModEntry.Instance.HalfShield.Status, 1);
}
