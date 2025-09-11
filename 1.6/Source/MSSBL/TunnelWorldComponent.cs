using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace MSSBL;

public class TunnelWorldComponent(World world) : WorldComponent(world)
{
    public List<Tunnel> Tunnels;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref Tunnels, "Tunnels", LookMode.Deep);
    }
}