using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using VEF.Factions;
using VEF.Factions.GameComponents;
using Verse;

namespace MSSBL;

[HarmonyPatch(typeof(WorldComponent_FactionGoodwillImpactManager))]
public static class WorldComponent_FactionGoodwillImpactManager_Patch
{
    public static Lazy<FieldInfo> goodwillImpacts = new(()=>AccessTools.Field(typeof(WorldComponent_FactionGoodwillImpactManager), "goodwillImpacts"));
    
    public class RaidGoodwillImpactDelayed : GoodwillImpactDelayed
    {
        public override void DoImpact()
        {
            Map map = Find.AnyPlayerHomeMap;
            IncidentParms parms = new IncidentParms
            {
                target = Find.AnyPlayerHomeMap,
                points = StorytellerUtility.DefaultThreatPointsNow(map) * 1.5f,
                faction = factionToImpact,
                raidStrategy = RaidStrategyDefOf.ImmediateAttack,
                raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkInGroups,
                spawnCenter = map.Center
            };
            IncidentDefOf.RaidEnemy.Worker.TryExecute(parms);
        }

        public static RaidGoodwillImpactDelayed FromGoodwillImpactDelayed(GoodwillImpactDelayed goodwillImpactDelayed)
        {
            return new RaidGoodwillImpactDelayed
            {
                factionToImpact = goodwillImpactDelayed.factionToImpact,
                goodwillImpact = goodwillImpactDelayed.goodwillImpact,
                historyEvent = goodwillImpactDelayed.historyEvent,
                impactInTicks = goodwillImpactDelayed.impactInTicks,
                letterLabel = goodwillImpactDelayed.letterLabel,
                letterDesc = goodwillImpactDelayed.letterDesc,
                relationInfoKey = goodwillImpactDelayed.relationInfoKey,
                letterType = goodwillImpactDelayed.letterType
            };
        }
    }
    
    [HarmonyPatch(nameof(WorldComponent_FactionGoodwillImpactManager.ImpactFactionGoodwill))]
    [HarmonyPostfix]
    public static void Postfix(WorldComponent_FactionGoodwillImpactManager __instance, GoodwillImpactDelayed goodwillImpact)
    {
        GoodwillImpactDelayed impact = RaidGoodwillImpactDelayed.FromGoodwillImpactDelayed(goodwillImpact);

        if (Rand.Chance(MSSBLMod.settings.chanceToRaid))
        {
            List<GoodwillImpactDelayed> impacts =
                goodwillImpacts.Value.GetValue(__instance) as List<GoodwillImpactDelayed>;
            impacts?.Add(impact);
        }
    }
}