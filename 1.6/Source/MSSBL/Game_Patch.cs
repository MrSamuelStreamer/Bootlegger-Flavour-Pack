using HarmonyLib;
using Verse;

namespace MSSBL;

[HarmonyPatch(typeof(Game))]
public static class Game_Patch
{
    [HarmonyPatch(nameof(Game.InitNewGame))]
    [HarmonyPrefix]
    public static void InitNewGame(Game __instance)
    {
        if (__instance.InitData.startingTile.Layer.IsRootSurface)
        {
            __instance.InitData.startingTile.Tile.PrimaryBiome = MSSBLDefOf.MSSBL_City_PlayerHome;
        }
    }
}