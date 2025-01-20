using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class YC : Item
{
    public override void OnOtherShipMoves(State state, Combat combat, Part part)
    {
        combat.QueueImmediate(new AStatus
        {
            status = ModEntry.Instance.KokoroApi.OxidationStatus.Status,
            statusAmount = 1,
            timer = 0,
            targetPlayer = !playerOwned
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.OxidationStatus.Status, 1);
}
