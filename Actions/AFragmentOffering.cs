using FSPRO;
using JyGein.PartCrystals.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Actions;

public class AFragmentOffering : CardAction
{
    public int amount = 3;
    public int amountSize = 2;
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        timer = 0.0;
        return new FragmentReward
        {
            fragments = FragmentReward.GetOffering(s, amount, amountSize)
        };
    }

    public override Icon? GetIcon(State s)
    {
        return new Icon(StableSpr.icons_heal, 0, Colors.heal);
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        return new List<Tooltip>();
    }
}
