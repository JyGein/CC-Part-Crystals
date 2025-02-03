using HarmonyLib;
using Nickel;
using JyGein.PartCrystals.dumb_stupid_idiot_strings;
using JyGein.PartCrystals.Fragments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Features;

internal sealed class AttachStuffToEnemies
{
    public static void Begin(State s, AI ai, Combat c, bool railCannon = false)
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
                parts[0].SetAttachables([]);
                parts[2].SetAttachables([new OrangeFragment()]);
                parts[3].SetAttachables([new RedFragment()]);
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
                parts[6].SetAttachables([new YO()]);
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
                parts[0].SetAttachables([new RedFragment()]);
                parts[1].SetAttachables([]);
                parts[2].SetAttachables([new RO()]);
                parts[3].SetAttachables([new OrangeFragment()]);
                break;
            case PaybackBruiserZ1:
                parts[0].SetAttachables([new BlueFragment()]);
                parts[1].SetAttachables([new BM()]);
                parts[2].SetAttachables([new BlueFragment()]);
                parts[3].SetAttachables([new BlueFragment(), new RM()]);
                parts[4].SetAttachables([new BlueFragment()]);
                break;
            case CrystalBoss:
                List<Fragment> fragments3 = GetFragments(s, c.otherShip, parts.Count(p => p.type != PType.empty) * 2);
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
            case LightFighterZone2:
                parts[0].SetAttachables([new YellowFragment()]);
                parts[1].SetAttachables([new BlueFragment()]);
                parts[2].SetAttachables([new RR()]);
                parts[3].SetAttachables([new MO()]);
                parts[4].SetAttachables([new YellowFragment()]);
                break;
            case GoliathDefender:
                parts[0].SetAttachables([new BY()]);
                parts[1].SetAttachables([new BR()]);
                parts[2].SetAttachables([new MagentaFragment()]);
                parts[3].SetAttachables([new MagentaFragment()]);
                parts[4].SetAttachables([new MagentaFragment()]);
                parts[5].SetAttachables([new BY()]);
                break;
            case BinaryBaddie:
                parts[0].SetAttachables([new BlueFragment()]);
                parts[1].SetAttachables([new RedFragment()]);
                parts[2].SetAttachables([new MagentaFragment()]);
                parts[4].SetAttachables([new GM(), new YellowFragment()]);
                parts[5].SetAttachables([new GM(), new YellowFragment(), new YellowFragment()]);
                break;
            case WideMissiler:
                List<int> fragPos = new List<int>([1, 2, 3, 4]).Shuffle(s.rngAi).Take(2).ToList();
                parts[0].SetAttachables([new YellowFragment(), .. (List<AttachableToPart>)(fragPos.Contains(1) ? [new RedFragment()] : [])]);
                parts[1].SetAttachables([new GO()]);
                parts[2].SetAttachables(fragPos.Contains(2) ? [new BlueFragment()] : []);
                parts[3].SetAttachables(fragPos.Contains(3) ? [new BlueFragment()] : []);
                parts[4].SetAttachables([new CG()]);
                parts[5].SetAttachables([new YellowFragment(), .. (List<AttachableToPart>)(fragPos.Contains(4) ? [new RedFragment()] : [])]);
                break;
            case LockdownBruiser:
                parts[0].SetAttachables([new YC()]);
                parts[1].SetAttachables([new RedFragment()]);
                parts[2].SetAttachables([new MagentaFragment(), new BlueFragment(), new GreenFragment()]);
                parts[3].SetAttachables([new YO()]);
                break;
            case OxygenLeakGuy:
                parts[0].SetAttachables([new RC(), new RC()]);
                parts[1].SetAttachables([new MagentaFragment(), new MagentaFragment()]);
                parts[3].SetAttachables([new MagentaFragment(), new MagentaFragment()]);
                parts[5].SetAttachables([new RC(), new RC()]);
                break;
            case WideCruiserAlt:
                parts[0].SetAttachables([new RY()]);
                parts[1].SetAttachables([new GreenFragment()]);
                parts[2].SetAttachables([new BlueFragment()]);
                parts[3].SetAttachables([new MagentaFragment()]);
                parts[4].SetAttachables([new RedFragment()]);
                parts[5].SetAttachables([new MagentaFragment()]);
                parts[6].SetAttachables([new BlueFragment()]);
                parts[7].SetAttachables([new GreenFragment()]);
                parts[8].SetAttachables([new RY()]);
                break;
            case Knight:
                parts[0].SetAttachables([new YellowFragment(), new YellowFragment()]);
                parts[1].SetAttachables([new RedFragment(), new RedFragment()]);
                parts[2].SetAttachables([new YM(), new CO()]);
                parts[3].SetAttachables([new BlueFragment(), new BlueFragment()]);
                parts[4].SetAttachables([new YellowFragment(), new YellowFragment()]);
                break;
            case CrystalMiniboss:
                List<Item> items2 = GetItems(s, c.otherShip, 4);
                List<Fragment> fragments4 = GetFragments(s, c.otherShip, 4);
                parts[1].SetAttachables([items2[0], fragments4[0]]);
                parts[3].SetAttachables([items2[1], fragments4[1]]);
                parts[5].SetAttachables([items2[2], fragments4[2]]);
                parts[7].SetAttachables([items2[3], fragments4[3]]);
                break;
            case RailCannon:
                if (railCannon)
                {
                    switch (c.otherShip.Get(Status.survive))
                    {
                        case int x when x >= 2:
                            parts[0].SetAttachables([new MM()]);
                            parts[1].SetAttachables([new GreenFragment()]);
                            parts[2].SetAttachables([new GreenFragment()]);
                            parts[3].SetAttachables([new RedFragment()]);
                            parts[5].SetAttachables([new MagentaFragment()]);
                            parts[7].SetAttachables([new RedFragment()]);
                            parts[8].SetAttachables([new BC()]);
                            break;
                        case int x when x == 1:
                            parts[1].SetAttachables([new BC()]);
                            parts[3].SetAttachables([new RedFragment()]);
                            parts[4].SetAttachables([new MagentaFragment()]);
                            parts[6].SetAttachables([new GreenFragment()]);
                            parts[7].SetAttachables([new GG()]);
                            parts[8].SetAttachables([new RedFragment()]);
                            parts[9].SetAttachables([new RedFragment()]);
                            break;
                        case int x when x <= 0:
                            parts[1].SetAttachables([new RedFragment()]);
                            parts[2].SetAttachables([new RedFragment()]);
                            parts[3].SetAttachables([new RR()]);
                            parts[4].SetAttachables([new RedFragment()]);
                            parts[5].SetAttachables([new RedFragment()]);
                            parts[6].SetAttachables([new RG()]);
                            parts[7].SetAttachables([new RedFragment()]);
                            parts[10].SetAttachables([new BC()]);
                            break;
                    }
                }
                else
                {
                    parts[0].SetAttachables([new MM()]);
                    parts[1].SetAttachables([new GreenFragment()]);
                    parts[2].SetAttachables([new GreenFragment()]);
                    parts[3].SetAttachables([new RedFragment()]);
                    parts[5].SetAttachables([new MagentaFragment()]);
                    parts[7].SetAttachables([new RedFragment()]);
                    parts[8].SetAttachables([new BC()]);
                    break;
                }
                break;
            case AsteroidBoss:
                parts[0].SetAttachables([new YellowFragment(), new MagentaFragment()]);
                parts[1].SetAttachables([new MM()]);
                parts[2].SetAttachables([new MC()]);
                parts[3].SetAttachables([new YellowFragment(), new MagentaFragment()]);
                break;
            case PirateBoss:
                parts[0].SetAttachables([new BO(), new YellowFragment(), new YellowFragment()]);
                parts[1].SetAttachables([new RedFragment()]);
                parts[2].SetAttachables([new RG()]);
                parts[3].SetAttachables([]);
                parts[4].SetAttachables([new RedFragment()]);
                parts[5].SetAttachables([new BR()]);
                parts[6].SetAttachables([new BR()]);
                parts[7].SetAttachables([new RG()]);
                parts[8].SetAttachables([]);
                parts[9].SetAttachables([new MagentaFragment()]);
                parts[10].SetAttachables([new RedFragment()]);
                parts[11].SetAttachables([new YellowFragment(), new YellowFragment()]);
                break;
            case Z2BossMechaPossum:
                parts[0].SetAttachables([new YellowFragment()]);
                parts[1].SetAttachables([new RedFragment()]);
                parts[2].SetAttachables([new YM()]);
                parts[3].SetAttachables([new MM(), new BB()]);
                parts[4].SetAttachables([new RG()]);
                parts[5].SetAttachables([new MM(), new BB()]);
                parts[6].SetAttachables([new YM()]);
                parts[7].SetAttachables([new RedFragment()]);
                parts[8].SetAttachables([new YellowFragment()]);
                break;
            case LightFighterZone3:
                parts[0].SetAttachables([new MagentaFragment()]);
                parts[1].SetAttachables([new RR(), new RR()]);
                parts[2].SetAttachables([new MO()]);
                parts[3].SetAttachables([new RR(), new RR()]);
                parts[4].SetAttachables([new MagentaFragment()]);
                break;
            case Shopkeep:
                parts[0].SetAttachables([new BM(), new YY()]);
                parts[1].SetAttachables([new BM(), new YY()]);
                parts[2].SetAttachables([new BM(), new YY()]);
                break;
            case LightFighter:
                parts[0].SetAttachables([new OrangeFragment(), new CyanFragment()]);
                parts[1].SetAttachables([new MagentaFragment()]);
                parts[2].SetAttachables([new RedFragment()]);
                parts[3].SetAttachables([new BlueFragment(), new GreenFragment()]);
                parts[4].SetAttachables([new YellowFragment()]);
                break;
            default: //AKA unknown/unimplemented enemy
                List<Fragment> fragments1 = GetFragments(s, c.otherShip, 4);
                List<Item> items1 = GetItems(s, c.otherShip, itemNum);
                ThrowAttachablesOntoShip(items1, fragments1, c.otherShip, s);
                break;
        }
        c.otherShip.SetShipAttachablesPlayerOwned(false);
    }

    private static List<Fragment> GetFragments(State s, Ship ship, int amt = 7, bool noRed = false)
    {
        List<Fragment> list = [];
        List<Type> fragmentTypes = ModEntry.Instance.fragmentTypes;
        if (ship.GetMaxShield() <= 0) fragmentTypes.Remove(typeof(BlueFragment));
        if (ship.immovable) fragmentTypes.Remove(typeof(YellowFragment));
        if (noRed) fragmentTypes.Remove(typeof(RedFragment));
        for (int i = 0; i < amt; i++) list.Add((Fragment)AccessTools.CreateInstance(fragmentTypes.Shuffle(s.rngShuffle).First()));
        return list;
    }

    private static List<Item> GetItems(State s, Ship ship, int amt, bool noRed = false)
    {
        List<Item> list = [];
        HashSet<Type> set = [];
        ModEntry.Instance.fragmentFragmentToItem.Select(dd => dd.Value).ToList().ForEach(ld => ld.Select(d => d.Value).ToList().ForEach(t => set.Add(t)));
        if (ship.GetMaxShield() <= 0) set.RemoveWhere(t => t.Name.Contains('B'));
        if (ship.immovable) set.RemoveWhere(t => t.Name.Contains('Y'));
        if (noRed) set.RemoveWhere(t => t.Name.Contains('R'));
        for (int i = 0; i < amt; i++) list.Add((Item)AccessTools.CreateInstance(set.Shuffle(s.rngShuffle).First()));
        return list;
    }

    public static void ThrowAttachablesOntoShip(List<Item> items, List<Fragment> fragments, Ship ship, State s)
    {
        while (items.Count > 0 && ship.CanAttachItem())
        {
            if (ship.parts.Where(p => p.GetAttachables().Size() >= 3).All(p => p.type == PType.missiles) && items[0].GetBaseFragmentTypes().Contains(typeof(RedFragment)))
            {
                items[0] = GetItems(s, ship, 1, true).Single();
                continue;
            }
            Part p = ship.parts.Where(p => p.GetAttachables().Select(a => a.GetSize()).Sum() <= 2 && p.type != PType.empty).Shuffle(s.rngShuffle).First();
            p.SetAttachables([.. p.GetAttachables(), items.Pop()]);
        }
        while (fragments.Count > 0 && ship.CanAttachFragment())
        {
            if (ship.parts.Where(p => p.GetAttachables().Size() >= 4).All(p => p.type == PType.missiles) && fragments[0].GetType() == typeof(RedFragment))
            {
                fragments[0] = GetFragments(s, ship, 1, true).Single();
                continue;
            }
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
    public static List<AttachableToPart> AppendRange(this List<AttachableToPart> list1, List<AttachableToPart> list2)
        => list1.Concat(list2).ToList();
}
