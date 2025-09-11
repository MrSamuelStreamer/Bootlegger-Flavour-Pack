using System.Collections.Generic;
using System.Threading;
using RimWorld.Planet;
using Verse;

namespace MSSBL;

public class Tunnel: IExposable, ILoadReferenceable
{
    public Settlement Settlement;
    public Map Map;

    public List<PlanetTile> pathNodes;
    
    public bool UnderConstruction;
    
    private int loadID = -1;
    
    public void InitializeAfterClone() => loadID = Find.UniqueIDsManager.GetNextBillID();

    public Tunnel(Settlement settlement, Map sourceMap)
    {
        Settlement = settlement;
        Map = sourceMap;
        InitializeAfterClone();
        ThreadPool.QueueUserWorkItem(_ =>
        {
            WorldPath path = Map.Tile.Layer.Pather.FindPath(Map.Tile, Settlement.Tile, null, null);
            pathNodes = new List<PlanetTile>(path.NodesReversed);
        });
    }
    
    public void ExposeData()
    {
        Scribe_References.Look(ref Settlement, "settlement");
        Scribe_References.Look(ref Map, "map");
        Scribe_Collections.Look(ref pathNodes, "pathNodes", LookMode.Reference);
        Scribe_Values.Look(ref UnderConstruction, "underConstruction", true);
    }

    public string GetUniqueLoadID() => $"MSSBL.TunnelBill_{Settlement.GetUniqueLoadID()}_{loadID.ToString()}";
}