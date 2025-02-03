using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class BM : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(BlueFragment), typeof(MagentaFragment)];

    private bool hasGrantedShield = false;

    public override void OnCombatStart(State state, Combat combat, Part part)
    {
        base.OnCombatStart(state, combat, part);
        if (playerOwned)
        {
            hasGrantedShield = false;
        }
        else
        {
            combat.Queue(new AStatus
            {
                status = Status.perfectShield,
                statusAmount = 1,
                targetPlayer = playerOwned,
                timer = 0
            });
        }
    }
    public override void OnTurnStart(State state, Combat combat, Part part)
    {
        base.OnTurnStart(state, combat, part);
        if (!hasGrantedShield && playerOwned)
        {
            combat.Queue(new AStatus
            {
                status = Status.perfectShield,
                statusAmount = 1,
                targetPlayer = playerOwned,
                timer = 0
            });
            hasGrantedShield = true;
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(Status.perfectShield, 1);
}
