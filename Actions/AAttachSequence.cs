using FSPRO;
using PartCrystals.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Actions;

public class AAttachSequence : CardAction
{
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        timer = 0.0;
        ShipUpgrades route = new();
        route.SetIsSpecialAttachSequence(true);
        return route;
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
