using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Features;

public abstract class AttachableToPart
{
    public bool playerOwned = true;
    public virtual int GetSize() => 0;
    public virtual void OnCombatStart(State state, Combat combat, Part part) { }
    public virtual void OnTurnStart(State state, Combat combat, Part part) { }
    public virtual void OnTurnEnd(State state, Combat combat, Part part) { }
    public virtual void OnPartHit(State state, Combat combat, Part part) { }
    public virtual void OnShipShoots(State state, Combat combat, Part part) { }
    public abstract List<Tooltip> GetTooltips();
    public abstract void Render(G g, Vec restingPosition, bool autoFocus = false);
    public abstract UIKey UIKey();
    public abstract Spr GetSprite();
}
