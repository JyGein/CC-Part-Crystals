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
        switch (ai)
        {
            case LightFighter:
                List<Fragment> fragments = ModEntry.Instance.fragmentTypes.Select(t => (Fragment)AccessTools.CreateInstance(t)).ToList();
                fragments.ForEach(f => f.playerOwned = false);
                List<Item> items = GetItems(s, itemNum);
                ThrowAttachablesOntoShip(items, fragments, c.otherShip, s);
                break;
            default:
                List<Fragment> fragments1 = GetFragments(s, c);
                List<Item> items1 = GetItems(s, itemNum);
                ThrowAttachablesOntoShip(items1, fragments1, c.otherShip, s);
                break;
        }
    }

    private static List<Fragment> GetFragments(State s, Combat c, int amt = 7)
    {
        List<Fragment> list = [];
        List<Type> fragmentTypes = ModEntry.Instance.fragmentTypes;
        if (c.otherShip.GetMaxShield() <= 0) fragmentTypes.Remove(typeof(BlueFragment));
        if (c.otherShip.immovable) fragmentTypes.Remove(typeof(YellowFragment));
        for (int i = 0; i < amt; i++)
        {
            Fragment fragment = (Fragment)AccessTools.CreateInstance(ModEntry.Instance.fragmentTypes.Shuffle(s.rngShuffle).First());
            fragment.playerOwned = false;
            list.Add(fragment);
        }
        return list;
    }

    private static List<Item> GetItems(State s, int amt)
    {
        List<Item> list = [];
        HashSet<Type> set = [];
        ModEntry.Instance.fragmentFragmentToItem.Select(dd => dd.Value).ToList().ForEach(ld => ld.Select(d => d.Value).ToList().ForEach(t => set.Add(t)));
        for (int i = 0; i < amt; i++)
        {
            Item item = (Item)AccessTools.CreateInstance(set.Shuffle(s.rngShuffle).First());
            item.playerOwned = false;
            list.Add(item);
        }
        return list;
    }

    public static void ThrowAttachablesOntoShip(List<Item> items, List<Fragment> fragments, Ship ship, State s)
    {
        while (items.Count > 0 && ship.CanAttachItem())
        {
            Part p = ship.parts.Where(p => p.GetAttachables().Select(a => a.GetSize()).Sum() <= 2).Shuffle(s.rngShuffle).First();
            p.SetAttachables([.. p.GetAttachables(), items.Pop()]);
        }
        while (fragments.Count > 0 && ship.CanAttachFragment())
        {
            Part p = ship.parts.Where(p => p.GetAttachables().Select(a => a.GetSize()).Sum() <= 1).Shuffle(s.rngShuffle).First();
            p.SetAttachables([.. p.GetAttachables(), fragments.Pop()]);
        }
    }
}

internal static partial class AttachableToPartExt
{
    public static int GetShipMaxSize(this Ship ship)
        => ship.parts.Count(p => p.type != PType.empty) * 4;
    public static bool CanAttachItem(this Ship ship)
        => ship.parts.Any(p => p.GetAttachables().Select(a => a.GetSize()).Sum() <= 2);
    public static bool CanAttachFragment(this Ship ship)
        => ship.parts.Any(p => p.GetAttachables().Select(a => a.GetSize()).Sum() <= 1);
    public static T Pop<T>(this List<T> list)
    {
        T item = list.First();
        list.RemoveAt(0);
        return item;
    }
}
