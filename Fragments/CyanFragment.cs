using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class CyanFragment : Fragment
{
    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        base.OnPartHit(state, combat, part, damageDone);
        combat.QueueImmediate(new AStatus
        {
            status = ModEntry.Instance.KokoroApi.OxidationStatus.Status,
            statusAmount = playerOwned ? 3 : 1,
            targetPlayer = !playerOwned,
            timer = 0
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.OxidationStatus.Status, 0);
}
