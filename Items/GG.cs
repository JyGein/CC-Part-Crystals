using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class GG : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(GreenFragment), typeof(GreenFragment)];

    public int storedHull = 0;
    public override void OnCombatStart(State state, Combat combat, Part part)
    {
        base.OnCombatStart(state, combat, part);
        if (playerOwned) return;
        combat.otherShip.hullMax += 3;
    }
    public override void OnPartAttached(State state, Part part)
    {
        base.OnPartAttached(state, part);
        state.ship.hullMax += 3;
        state.ship.hull += storedHull;
    }
    public override void OnPartDetached(State state, Part part)
    {
        base.OnPartDetached(state, part);
        if (state.ship.hullMax - 3 < state.ship.hull) storedHull = state.ship.hull - (state.ship.hullMax - 3);
        state.ship.hullMax -= 3;
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [new TTGlossary("action.hullMax", 3)];
}
