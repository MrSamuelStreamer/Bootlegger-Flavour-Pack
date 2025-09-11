using RimWorld.Planet;
using Verse;

namespace MSSBL;

public class TunnelBill: IExposable, ILoadReferenceable
{
    public Settlement Settlement;
    public Map Map;
    public float cost;
    public float progress;
    public Tunnel tunnel;
    
    private int loadID = -1;
    
    public void InitializeAfterClone() => loadID = Find.UniqueIDsManager.GetNextBillID();

    public TunnelBill(Settlement settlement, Map sourceMap, float cost)
    {
        Settlement = settlement;
        Map = sourceMap;
        this.cost = cost;
        tunnel = new Tunnel(settlement, sourceMap);
        Find.World.GetComponent<TunnelWorldComponent>().Tunnels.Add(tunnel);
        InitializeAfterClone();
    }
    
    public void ExposeData()
    {
        Scribe_References.Look(ref Settlement, "settlement");
        Scribe_References.Look(ref Map, "map");
        Scribe_Values.Look(ref cost, "cost", -1f);
        Scribe_Values.Look(ref progress, "progress", -1f);
        Scribe_References.Look(ref tunnel, "tunnel");
    }

    public string GetUniqueLoadID() => $"MSSBL.TunnelBill_{Settlement.GetUniqueLoadID()}_{loadID.ToString()}";
}