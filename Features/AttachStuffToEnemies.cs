using HarmonyLib;
using Nickel;
using PartCrystals.dumb_stupid_idiot_strings;
using PartCrystals.Fragments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Features;

internal sealed class AttachStuffToEnemies
{
    public static void Begin(State s, AI ai, Combat c)
    {
        int itemNum = 0;
        switch (s.map)
        {
            case MapFirst:
                break;
            case MapLawless:
                itemNum += 2;
                break;
            case MapThree:
                itemNum += 4;
                break;
            default:
                return;
        }
        if (s.map.markers[s.map.currentLocation].contents is MapBattle mapBattle)
        {
            switch (mapBattle.battleType)
            {
                case BattleType.Elite:
                    itemNum += 1;
                    break;
                case BattleType.Boss:
                    itemNum += 2;
                    break;
            }
        }
        List<Part> parts = c.otherShip.parts;
        switch (ai)
        {
            case DroneDropperZ1:
                parts[0].SetAttachables([new BB()]);
                parts[1].SetAttachables([new MagentaFragment(), new MagentaFragment()]);
                parts[2].SetAttachables([new BB()]);
                parts[3].SetAttachables([new RedFragment()]);
                break;
            case LightScouter:
                parts[0].SetAttachables([new YellowFragment(), new YellowFragment(), new YellowFragment(), new YellowFragment()]);
                parts[1].SetAttachables([]);
                parts[2].SetAttachables([new YellowFragment(), new YellowFragment(), new YellowFragment(), new YellowFragment()]);
                break;
            case Wizard:
                parts[0].SetAttachables([new OrangeFragment()]);
                parts[2].SetAttachables([new OrangeFragment(), new OrangeFragment()]);
                parts[3].SetAttachables([new OrangeFragment(), new RedFragment()]);
                parts[4].SetAttachables([new CO()]);
                break;
            case MediumFighter:
                parts[0].SetAttachables([new GreenFragment()]);
                parts[1].SetAttachables([new GG()]);
                parts[2].SetAttachables([new GreenFragment()]);
                parts[3].SetAttachables([new GreenFragment()]);
                parts[4].SetAttachables([new GreenFragment()]);
                parts[5].SetAttachables([new GreenFragment()]);
                break;
            case DualDroneBaddie:
                List<Fragment> fragments2 = new List<Fragment> { new BlueFragment(), new GreenFragment(), new CyanFragment(), new OrangeFragment() }.Shuffle(s.rngAi).ToList();
                parts[0].SetAttachables([fragments2[0]]);
                parts[2].SetAttachables([fragments2[1]]);
                parts[3].SetAttachables([new GB()]);
                parts[4].SetAttachables([fragments2[2]]);
                parts[6].SetAttachables([fragments2[3]]);
                break;
            case HeavyFighter:
                parts[0].SetAttachables([new GY()]);
                parts[1].SetAttachables([new RedFragment()]);
                parts[2].SetAttachables([]);
                parts[3].SetAttachables([new OrangeFragment()]);
                parts[4].SetAttachables([]);
                parts[5].SetAttachables([new RedFragment()]);
                parts[5].SetAttachables([new YO()]);
                break;
            case SimpleMissiler:
                parts[0].SetAttachables([new YellowFragment()]);
                parts[1].SetAttachables([new RR()]);
                parts[2].SetAttachables([]);
                parts[3].SetAttachables([new MM()]);
                parts[4].SetAttachables([new YellowFragment()]);
                break;
            case FootballFoe:
                parts[0].SetAttachables([new OO(), new OO()]);
                parts[1].SetAttachables([new CC(), new CC()]);
                parts[2].SetAttachables([new CO(), new CO()]);
                break;
            case SogginsEvent:
                parts[0].SetAttachables([new OrangeFragment()]);
                parts[1].SetAttachables([new CyanFragment()]);
                parts[2].SetAttachables([new OrangeFragment()]);
                parts[3].SetAttachables([new CyanFragment()]);
                parts[4].SetAttachables([new OrangeFragment()]);
                break;
            case WideCruiser:
                parts[0].SetAttachables([]);
                parts[1].SetAttachables([new RO()]);
                parts[2].SetAttachables([new RedFragment()]);
                parts[3].SetAttachables([new RedFragment()]);
                parts[4].SetAttachables([]);
                parts[5].SetAttachables([new RedFragment()]);
                parts[6].SetAttachables([new RedFragment()]);
                parts[7].SetAttachables([new RO()]);
                parts[8].SetAttachables([]);
                break;
            case DrakePirate:
                parts[0].SetAttachables([new RO()]);
                parts[1].SetAttachables([]);
                parts[2].SetAttachables([new RO()]);
                parts[3].SetAttachables([new RO()]);
                break;
            case PaybackBruiserZ1:
                parts[0].SetAttachables([new BlueFragment()]);
                parts[1].SetAttachables([new BM()]);
                parts[2].SetAttachables([new BlueFragment()]);
                parts[3].SetAttachables([new BlueFragment(), new RM()]);
                parts[4].SetAttachables([new BlueFragment()]);
                break;
            case CrystalBoss:
                List<Fragment> fragments3 = GetFragments(s, c, parts.Count(p => p.type != PType.empty) * 2);
                ThrowAttachablesOntoShip([], fragments3, c.otherShip, s);
                break;
            case Z1BossFreeze:
                parts[0].SetAttachables([new RedFragment()]);
                parts[1].SetAttachables([new YM()]);
                parts[2].SetAttachables([new MagentaFragment(), new GreenFragment()]);
                parts[3].SetAttachables([new RG()]);
                parts[4].SetAttachables([new BlueFragment(), new GreenFragment()]);
                parts[5].SetAttachables([new BlueFragment(), new GreenFragment()]);
                parts[6].SetAttachables([]);
                parts[7].SetAttachables([]);
                parts[8].SetAttachables([new YO()]);
                parts[9].SetAttachables([]);
                parts[10].SetAttachables([new RedFragment()]);
                break;
            case LightFighter:
                List<Fragment> fragments = ModEntry.Instance.fragmentTypes.Select(t => (Fragment)AccessTools.CreateInstance(t)).ToList();
                List<Item> items = [];
                ThrowAttachablesOntoShip(items, fragments, c.otherShip, s);
                break;
            default:
                List<Fragment> fragments1 = GetFragments(s, c);
                List<Item> items1 = GetItems(s, c, itemNum);
                ThrowAttachablesOntoShip(items1, fragments1, c.otherShip, s);
                break;
        }
        c.otherShip.SetShipAttachablesPlayerOwned(false);
    }

    private static List<Fragment> GetFragments(State s, Combat c, int amt = 7)
    {
        List<Fragment> list = [];
        List<Type> fragmentTypes = ModEntry.Instance.fragmentTypes;
        if (c.otherShip.GetMaxShield() <= 0) fragmentTypes.Remove(typeof(BlueFragment));
        if (c.otherShip.immovable) fragmentTypes.Remove(typeof(YellowFragment));
        for (int i = 0; i < amt; i++) list.Add((Fragment)AccessTools.CreateInstance(fragmentTypes.Shuffle(s.rngShuffle).First()));
        return list;
    }

    private static List<Item> GetItems(State s, Combat c, int amt)
    {
        List<Item> list = [];
        HashSet<Type> set = [];
        ModEntry.Instance.fragmentFragmentToItem.Select(dd => dd.Value).ToList().ForEach(ld => ld.Select(d => d.Value).ToList().ForEach(t => set.Add(t)));
        if (c.otherShip.GetMaxShield() <= 0) set.RemoveWhere(t => t.Name.Contains('B'));
        if (c.otherShip.immovable) set.RemoveWhere(t => t.Name.Contains('Y'));
        for (int i = 0; i < amt; i++) list.Add((Item)AccessTools.CreateInstance(set.Shuffle(s.rngShuffle).First()));
        return list;
    }

    public static void ThrowAttachablesOntoShip(List<Item> items, List<Fragment> fragments, Ship ship, State s)
    {
        while (items.Count > 0 && ship.CanAttachItem())
        {
            Part p = ship.parts.Where(p => p.GetAttachables().Select(a => a.GetSize()).Sum() <= 2 && p.type != PType.empty).Shuffle(s.rngShuffle).First();
            p.SetAttachables([.. p.GetAttachables(), items.Pop()]);
        }
        while (fragments.Count > 0 && ship.CanAttachFragment())
        {
            Part p = ship.parts.Where(p => p.GetAttachables().Select(a => a.GetSize()).Sum() <= 3 && p.type != PType.empty).Shuffle(s.rngShuffle).First();
            p.SetAttachables([.. p.GetAttachables(), fragments.Pop()]);
        }
    }
}

internal static partial class AttachableToPartExt
{
    public static int GetShipMaxSize(this Ship ship)
        => ship.parts.Count(p => p.type != PType.empty) * 4;
    public static bool CanAttachItem(this Ship ship)
        => ship.parts.Where(p => p.type != PType.empty).Any(p => p.GetAttachables().Select(a => a.GetSize()).Sum() <= 2);
    public static bool CanAttachFragment(this Ship ship)
        => ship.parts.Where(p => p.type != PType.empty).Any(p => p.GetAttachables().Select(a => a.GetSize()).Sum() <= 3);
    public static T Pop<T>(this List<T> list)
    {
        T item = list.First();
        list.RemoveAt(0);
        return item;
    }
    public static void SetShipAttachablesPlayerOwned(this Ship ship, bool playerOwned)
        => ship.parts.ForEach(p => p.GetAttachables().ForEach(a => a.playerOwned = playerOwned));
}
