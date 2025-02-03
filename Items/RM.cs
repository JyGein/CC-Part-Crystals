using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class RM : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(RedFragment), typeof(MagentaFragment)];

    public override void OnTurnStart(State state, Combat combat, Part part)
    {
        base.OnTurnStart(state, combat, part);
        combat.Queue(new AStatus
        {
            status = Status.libra,
            statusAmount = 1,
            targetPlayer = playerOwned,
            timer = 0
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(Status.libra, 1);
}
