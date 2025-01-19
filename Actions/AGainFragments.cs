using FSPRO;
using PartCrystals.Features;
using PartCrystals.Fragments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Actions;

public class AGainFragments : CardAction
{
    public List<AttachableToPart> fragments = [];

    public AGainFragments(List<Type> fragmentTypes)
    {
        foreach (Type type in fragmentTypes)
        {
            object obj = Activator.CreateInstance(type)!;
            if (obj is AttachableToPart attachable) fragments.Add(attachable);
        }
    }

    public AGainFragments(List<AttachableToPart> _fragments)
    {
        fragments = _fragments;
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        return fragments.SelectMany(f => f.GetTooltips()).Skip(1).ToList();
    }

    public override void Begin(G g, State s, Combat c)
    {
        s.SetPlayerAttachables([.. s.GetPlayerAttachables(), .. fragments]);
    }

    public override Icon? GetIcon(State s)
    {
        return new Icon(StableSpr.icons_heal, 0, Colors.heal);
    }
}
