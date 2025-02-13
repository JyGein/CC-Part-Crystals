using daisyowl.text;
using FSPRO;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JyGein.PartCrystals.Fragments;
using FMOD;
using JyGein.PartCrystals.Features;
using Nickel;

namespace JyGein.PartCrystals.Routes;

public class FragmentReward : Route, OnMouseDown
{
    public List<List<Fragment>> fragments = [];

    public bool canSkip = true;

    private readonly ParticleSystem particles = new()
    {
        blend = BlendMode.Screen,
        gradient =
        [
            Colors.white,
            Colors.black
        ]
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
        List<UnorderedList<Type>> chosenFragments = []; //choices.Shuffle(rngOverride ?? s.rngArtifactOfferings).Take(amount).ToList();
        for (int i = 0; i < amount; i++)
        {
            UnorderedList<Type> thisOne = new([]);
            int failSafe = 0;
            while ((thisOne.List.Count == 0 || chosenFragments.Any(uList => uList == thisOne)) && failSafe < 100)
            {
                List<Type> types = [];
                for (int f = 0; f < amountSize; f++) types.Add(ModEntry.Instance.fragmentTypes.Shuffle(rngOverride ?? s.rngArtifactOfferings).First());
                thisOne = new(types);
                failSafe++;
            };
            chosenFragments.Add(thisOne);
        }
        List<List<Fragment>> list1 = [];
        for (int i = 0; i < amount; i++)
        {
            List<Fragment> list2 = [];
            for (int f = 0; f < amountSize; f++)
            {
                list2.Add((Fragment)Activator.CreateInstance(chosenFragments[i].List[f])!);
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
        bool flag = fragments.All(list => list.Count == 1);
        string str = ModEntry.Instance.Localizations.Localize(["uiText", "fragmentReward", "title", flag ? "one" : "other"]);
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
            List<Fragment> fragmentList = fragments[i];
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
                g.tooltips.Add(xy + new Vec(num + 3, 2.0), fragmentList.SelectMany(f => f.GetTooltips()).Skip(1));
            }
            Vec vec = xy + new Vec(0.0, box.IsHover() ? 1 : 0);
            Vec vec2 = vec + new Vec(14.0, 14.0);
            if (fragmentList.Count == 1) Draw.Sprite(fragmentList[0].GetSprite(), (int)(vec2.x - 3.0), (int)(vec2.y - 4.0));
            else fragmentList.WithIndex().ForEach((f) => Draw.Sprite(f.Item1.GetSprite(), (int)(vec2.x - 7.0) + ((13 - 5) / (fragmentList.Count - 1) * f.Item2), (int)(vec2.y - 7.0) + ((13 - 7) / (fragmentList.Count - 1) * f.Item2)));
            string locName = string.Join(" and ", fragmentList.Select(f => $"<c={Fragment.FragmentColors[f.GetType()]}>{f.Name().Remove(f.Name().Length - 9)}</c>"));
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
