using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
//using PartCrystals.Actions;
//using PartCrystals.Cards;
using PartCrystals.External;
using PartCrystals.dumb_stupid_idiot_strings;
using System.Reflection;
using PartCrystals.Features;
using Microsoft.Xna.Framework.Graphics;

namespace PartCrystals.Fragments;

public class Fragment : AttachableToPart
{
    public int v = Mutil.NextRandInt();
    public static readonly UK FragmentUK = ModEntry.Instance.Helper.Utilities.ObtainEnumCase<UK>();

    public Vec? animation;

    public Vec lastScreenPos;

    public override int GetSize() => 1;

    public virtual List<Tooltip>? GetExtraTooltips()
        => null;

    public string Key()
    {
        return GetType().Name;
    }

    public override Spr GetSprite()
    {
        return ModEntry.Instance.FragmentSprites[Key()].Sprite;
    }

    public string Name()
    {
        return ModEntry.Instance.Localizations.Localize(["fragment", Key(), "name"]);
    }

    public string Desc()
    {
        return ModEntry.Instance.Localizations.Localize(["fragment", Key(), playerOwned ? "playerDesc" : "enemyDesc"]);
    }

    public override UIKey UIKey()
    {
        return new UIKey(FragmentUK, v, Key());
    }

    public override List<Tooltip> GetTooltips()
    {
        List<Tooltip> list = new()
        {
            new TTDivider(),
            new TTText($"<c=artifact>{Name().ToUpper()}</c>\n{Desc()}")
        };
        if (GetExtraTooltips() != null)
        {
            list.AddRange(GetExtraTooltips()!);
        }
        return list;
    }

    public override void Render(G g, Vec restingPosition, bool autoFocus = false)
    {
        UIKey? key = UIKey();
        Rect? rect = new Rect(0.0, 0.0, 11.0, 11.0) + restingPosition;
        bool autoFocus2 = autoFocus;
        Box box = g.Push(key, rect, null, autoFocus2);
        Vec vec = box.rect.xy;
        if (animation.HasValue)
        {
            animation = Mutil.LerpDelta(animation.Value, vec, 10.0, g.dt);
            if ((vec - animation.Value).len() < 0.5)
            {
                animation = null;
            }
            else
            {
                vec = animation.Value;
            }
        }
        Spr spr = GetSprite();
        lastScreenPos = vec;
        Vec vec2 = vec.round();
        Draw.Sprite(spr, vec2.x - 1.0, vec2.y - 1.0);
        if (box.IsHover())
        {
            Vec pos = vec2 + new Vec(20.0);
            g.tooltips.Add(pos, GetTooltips());
        }
        g.Pop();
    }
}
