using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class MagentaFragment : Fragment
{
    public override void OnTurnStart(State state, Combat combat, Part part)
    {
        combat.Queue(new AStatus
        {
            status = ModEntry.Instance.HalfTempShield.Status,
            statusAmount = 1,
            targetPlayer = playerOwned,
            timer = 0
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(ModEntry.Instance.HalfTempShield.Status, 1);
}
