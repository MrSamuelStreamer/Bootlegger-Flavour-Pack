using HarmonyLib;
using RimWorld;
using RimWorld.Planet;

namespace MSSBL;

[HarmonyPatch(typeof(SettleUtility))]
public static class SettleUtility_Patch
{
    [HarmonyPatch(nameof(SettleUtility.AddNewHome))]
    [HarmonyPostfix]
    public static void AddNewHome_Postfix(PlanetTile tile, Faction faction)
    {
        if (tile.Layer.IsRootSurface)
        {
            if (faction == Faction.OfPlayer) tile.Tile.PrimaryBiome = MSSBLDefOf.MSSBL_City_PlayerHome;
        }
    }
}