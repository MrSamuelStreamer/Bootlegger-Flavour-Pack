using HarmonyLib;
using Verse;

namespace MSSBL;

[HarmonyPatch(typeof(GameInitData))]
public static class GameInitData_Patch
{
    // [HarmonyPatch(nameof(GameInitData.ChooseRandomStartingTile))]
    // [HarmonyPostfix]
    // public static void ChooseRandomStartingTile_Postfix(GameInitData __instance)
    // {
    //     if (__instance.startingTile.Layer.IsRootSurface)
    //     {
    //         __instance.startingTile.Tile.PrimaryBiome = MSSBLDefOf.MSSBL_City_PlayerHome;
    //     }
    // }
}