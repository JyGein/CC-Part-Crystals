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
    public string Key() => GetType().Name;
    public virtual void OnCombatStart(State state, Combat combat, Part part) { }
    public virtual void OnTurnStart(State state, Combat combat, Part part) { }
    public virtual void OnOtherShipTurnStart(State state, Combat combat, Part part) { }
    public virtual void OnTurnEnd(State state, Combat combat, Part part) { }
    public virtual void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone) { }
    public virtual void BeforePartHit(State state, Combat combat, Part part, int incomingDamage) { }
    public virtual void OnPlayerShipShoots(State state, Combat combat, Part part) { }
    public virtual void AlterAttackFromPart(State state, Combat combat, Part part, AAttack aAttack) { }
    public virtual void AlterHullDamage(State state, Combat combat, Ship ship, ref int amt) { }
    public virtual void OnPartAttacks(State state, Combat combat, Part part) { }
    public virtual void OnShipMoves(State state, Combat combat, Part part) { }
    public virtual void OnOtherShipMoves(State state, Combat combat, Part part) { }
    public virtual void OnShipOverheats(State state, Combat combat, Part part) { }
    public virtual void OnPartDamages(State state, Combat combat, Part part, DamageDone damageDone, Ship ship) { }
    public virtual void OnPartAttached(State state, Part part) { }
    public virtual void OnPartDetached(State state, Part part) { }
    public abstract List<Tooltip> GetTooltips();
    public abstract void Render(G g, Vec restingPosition, bool autoFocus = false, OnMouseDown? onMouseDown = null, Color? color = null);
    public abstract UIKey UIKey();
    public abstract string Name();
    public abstract string Desc();
}
