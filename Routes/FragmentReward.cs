using daisyowl.text;
using FSPRO;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartCrystals.Fragments;
using FMOD;
using PartCrystals.Features;

namespace PartCrystals.Routes;

public class FragmentReward : Route, OnMouseDown
{
    public List<List<Fragment>> fragments = [];

    public bool canSkip = true;

    private ParticleSystem particles = new ParticleSystem
    {
        blend = BlendMode.Screen,
        gradient = new List<Color>
        {
            Colors.white,
            Colors.black
        }
    };
    public static readonly UK FragmentReward_FragmentUK = ModEntry.Instance.Helper.Utilities.ObtainEnumCase<UK>();
    public static readonly UK FragmentReward_Skip = ModEntry.Instance.Helper.Utilities.ObtainEnumCase<UK>();

    public override bool GetShowOverworldPanels()
    {
        return true;
    }

    public override bool CanBePeeked()
    {
        return true;
    }

    public static List<List<Fragment>> GetOffering(State s, int amount, int amountSize, Rand? rngOverride = null)
    {
        List<List<Type>> choices = new();
        foreach (Type type1 in ModEntry.Instance.fragmentTypes)
        {
            foreach (Type type2 in ModEntry.Instance.fragmentTypes)
            {
                choices.Add([type1, type2]);
            }
        }
        List<List<Fragment>> list1 = [];
        List<List<Type>> chosenFragments = choices.Shuffle(rngOverride ?? s.rngArtifactOfferings).Take(amount).ToList();
        for (int i = 0; i < amount; i++)
        {
            List<Fragment> list2 = [];
            for (int f = 0; f < amountSize; f++)
            {
                list2.Add((Fragment)Activator.CreateInstance(chosenFragments[i][f])!);
            }
            list1.Add(list2);
        }
        return list1;
    }

    public override void Render(G g)
    {
        if (fragments.Count == 0)
        {
            g.CloseRoute(this);
            return;
        }
        int num = 180;
        int num2 = 29;
        SharedArt.DrawEngineering(g);
        string str = ModEntry.Instance.Localizations.Localize(["uiText", "fragmentReward", "title"]);
        Font stapler = DB.stapler;
        Color? color = Colors.textMain;
        TAlign? align = TAlign.Center;
        Draw.Text(str, 240.0, 44.0, stapler, color, null, null, null, align);
        string str2 = ModEntry.Instance.Localizations.Localize(["uiText", "fragmentReward", "info"]);
        Color? color2 = Colors.textMain.gain(0.5);
        align = TAlign.Center;
        double? maxWidth = 300.0;
        Draw.Text(str2, 240.0, 69.0, null, color2, null, null, maxWidth, align);
        if (canSkip)
        {
            Vec localV = new Vec(210.0, 205.0);
            UIKey key = FragmentReward_Skip;
            string text = Loc.T("uiShared.btnSkipRewards");
            OnMouseDown onMouseDown = this;
            Color? boxColor = Colors.textMain.gain(0.5);
            SharedArt.ButtonText(g, localV, key, text, null, boxColor, inactive: false, onMouseDown, null, null, null, null, autoFocus: false, showAsPressed: false, gamepadUntargetable: false, hasDownState: false, null, null, null, null, 0, 60.0);
        }
        particles.Render(g.dt);
        for (int i = 0; i < fragments.Count; i++)
        {
            List<Fragment> fragmentPair = fragments[i];
            UIKey? key2 = new UIKey(FragmentReward_FragmentUK, i);
            Rect? rect = new Rect(240 - (int)(num / 2.0), 144.0 + Math.Floor((i - fragments.Count / 2.0) * (num2 - 2)), num, num2);
            OnMouseDown onMouseDown = this;
            Box box = g.Push(key2, rect, null, autoFocus: true, noHoverSound: false, gamepadUntargetable: false, ReticleMode.Quad, onMouseDown);
            Vec xy = box.rect.xy;
            Color? boxColor;
            Spr? id2 = box.IsHover() ? StableSpr.buttons_artifact_on : StableSpr.buttons_artifact;
            double x2 = xy.x;
            double y2 = xy.y;
            Draw.Sprite(id2, x2, y2, flipX: false, flipY: false, 0.0, null, null, null, null, null);
            if (box.IsHover())
            {
                g.tooltips.Add(xy + new Vec(num + 3, 2.0), fragmentPair.SelectMany(f => f.GetTooltips()).Skip(1));
            }
            Vec vec = xy + new Vec(0.0, box.IsHover() ? 1 : 0);
            Vec vec2 = vec + new Vec(14.0, 14.0);
            Vec vec3 = vec2;
            fragmentPair.WithIndex().ForEach((f) => Draw.Sprite(f.Item1.GetSprite(), (int)(vec3.x - 7.0) + 8 * f.Item2, (int)(vec3.y - 7.0) + 6 * f.Item2));
            string locName = string.Join(" and ", fragmentPair.Select(f => $"<c={Fragment.FragmentColors[f.GetType()]}>{f.Name().Remove(f.Name().Length - 9)}</c>"));
            double x3 = vec.x + 32.0;
            double y3 = vec.y + 11.0;
            boxColor = Colors.black;
            Draw.Text(locName, x3, y3, null, null, null, null, null, null, dontDraw: false, null, boxColor);
            g.Pop();
        }
    }

    public void OnMouseDown(G g, Box b)
    {
        if (b.key == FragmentReward_Skip)
        {
            Audio.Play(Event.Click);
            g.CloseRoute(this);
        }
        int? num = b.key?.ValueFor(FragmentReward_FragmentUK);
        if (!num.HasValue)
        {
            return;
        }
        int valueOrDefault = num.GetValueOrDefault();
        List<Fragment>? fragmentPair = fragments.ElementAtOrDefault(valueOrDefault);
        if (fragmentPair != null)
        {
            Audio.Play(Event.CardHandling);
            g.state.SetPlayerAttachables([.. g.state.GetPlayerAttachables(), .. fragmentPair]);
            g.CloseRoute(this);
        }
    }
}
