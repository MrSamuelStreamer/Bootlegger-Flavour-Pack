using Cities;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MSSBL;

[HarmonyPatch(typeof(GenCity))]
public static class GenCity_Patch
{
    [HarmonyPatch(nameof(GenCity.GetCityFaction))]
    [HarmonyPostfix]
    public static void GetCityFaction(Map map, ref Faction __result)
    {
        if (__result is null)
        {
            __result = Find.FactionManager.OfAncients;
            map.info.parent?.SetFaction(__result);
        }
    }
}