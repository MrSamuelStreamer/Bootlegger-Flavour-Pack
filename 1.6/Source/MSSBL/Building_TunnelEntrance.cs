using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MSSBL;

public class Building_TunnelEntrance: Building, IBillGiver
{
    public bool CurrentlyUsableForBills()
    {
        throw new System.NotImplementedException();
    }

    public bool UsableForBillsAfterFueling()
    {
        throw new System.NotImplementedException();
    }

    public void Notify_BillDeleted(Bill bill)
    {
        throw new System.NotImplementedException();
    }

    public BillStack BillStack { get; }
    public IEnumerable<IntVec3> IngredientStackCells { get; }
}