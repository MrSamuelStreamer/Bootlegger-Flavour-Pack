using Verse;

namespace MSSBL;

public class Building_TunnelEntrance: Building
{
    public TunnelBill CurrentBill;
    
    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Deep.Look(ref CurrentBill, "CurrentBill");
    }
}