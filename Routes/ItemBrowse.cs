using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daisyowl.text;
using FSPRO;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
using PartCrystals;
using PartCrystals.Fragments;
using PartCrystals.Features;

namespace PartCrystals.Routes;

public class ItemBrowse : Route, OnInputPhase, OnMouseDown
{
    public double scroll;

    public double scrollTarget;

    private Dictionary<UIKey, double> itemToScrollYCache = [];

    private Dictionary<int, Type> Vs = [];

    public ItemBrowse()
    {
        Dictionary<Type, Dictionary<Type, Type>> dictionary = ModEntry.Instance.fragmentFragmentToItem;
        dictionary.Select(dd => dd.Value).ToList().ForEach(ld => ld.Select(d => d.Value).ToList().ForEach(t => Vs[Mutil.NextRandInt()] = t));
    }

    public void OnInputPhase(G g, Box b)
    {
        if (Input.GetGpDown(Btn.B) || Input.GetKeyDown(Keys.Escape))
        {
            g.CloseRoute(this);
        }
    }

    public void OnMouseDown(G g, Box b)
    {
        if (b.key == StableUK.artifactBrowse_back)
        {
            Audio.Play(Event.Click);
            g.CloseRoute(this);
        }
    }

    public override void Render(G g)
    {
        G g2 = g;
        //HashSet<Type> set = [];
        //ModEntry.Instance.fragmentFragmentToItem.Select(dd => dd.Value).ToList().ForEach(ld => ld.Select(d => d.Value).ToList().ForEach(t => set.Add(t)));
        ScrollUtils.ReadScrollInputAndUpdate(g2.dt, 0, ref scroll, ref scrollTarget);
        if (Input.gamepadIsActiveInput && Input.currentGpKey.HasValue && itemToScrollYCache.TryGetValue(Input.currentGpKey.Value, out var value))
        {
            double num = 0.0 - value;
            scrollTarget = Mutil.Clamp(scrollTarget, num + 20.0, num + 120.0);
        }
        int num2 = (int)scroll;
        SharedArt.DrawEngineering(g2);
        string str = ModEntry.Instance.Localizations.Localize(["uiText", "itemBrowse", "title"]);
        double y = 24 + num2;
        Font stapler = DB.stapler;
        Color? color = Colors.textMain;
        TAlign? align = TAlign.Center;
        Draw.Text(str, 240.0, y, stapler, color, null, null, null, align);
        g2.Push(null, null, null, autoFocus: false, noHoverSound: false, gamepadUntargetable: false, ReticleMode.Quad, null, null, this);
        G g3 = g2;
        Vec localV = new Vec(413.0, 228.0);
        UIKey key = StableUK.artifactBrowse_back;
        string text = Loc.T("uiShared.btnBack");
        Btn? platformButtonHint = Btn.B;
        SharedArt.ButtonText(g3, localV, key, text, null, null, inactive: false, this, null, null, null, null, autoFocus: false, showAsPressed: false, gamepadUntargetable: false, hasDownState: false, platformButtonHint);
        double num3 = 0.0;
        int num4 = 70;
        itemToScrollYCache.Clear();
        int num5 = 0;
        int num6 = 0;
        int num7 = 7;
        int num8 = 23;
        int num9 = 23;
        int num10 = 0;
        G g4 = g2;
        Rect? rect = new Rect((g.mg.PIX_W - (num7 * num8 - (num8-11))) / 2, (double)(num4 + num2) + num3, 0.0, num3);
        Vec xy = g4.Push(null, rect).rect.xy;
        foreach ((int v, Type type) in Vs)
        {
            Item item = (Item)Activator.CreateInstance(type)!;
            item.v = v;
            if (num5 >= num7)
            {
                num5 = 0;
                num6++;
            }
            Vec vec = new Vec(num5++ * num8, num6 * num9 + num10);
            itemToScrollYCache[item.UIKey()] = vec.y + num3;
            item.Render(g2, vec, autoFocus: true);
        }
        num3 += (double)num10 + Math.Ceiling(Vs.Count / (double)num7) * (double)num9;
        g2.Pop();
        g2.Pop();
    }
}
