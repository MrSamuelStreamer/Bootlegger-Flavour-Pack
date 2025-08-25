using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Cities;
using HarmonyLib;
using Verse;
using Verse.AI.Group;

namespace MSSBL;

[StaticConstructorOnStartup]
public static class RimCityPatches
{
    static RimCityPatches()
    {
        // var assy = typeof(Cities.City).Assembly;

        var Room_Notify_ContainedThingSpawnedOrDespawned =
            AccessTools.TypeByName("Cities.Room_Notify_ContainedThingSpawnedOrDespawned");
        var Room_Notify_ContainedThingSpawnedOrDespawned_Prefix = AccessTools.Method(Room_Notify_ContainedThingSpawnedOrDespawned, "Prefix");

        var patch = AccessTools.Method(typeof(RimCityPatches),
            nameof(Cities_Room_Notify_ContainedThingSpawnedOrDespawned_TranspilerPatch));
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("MrSamuelStreamer.rimworld.MSSBL.main");
        harmony.PatchAll();

        // harmony.Patch(Room_Notify_ContainedThingSpawnedOrDespawned_Prefix, null, null, new HarmonyMethod(patch));
    }


    public static IEnumerable<CodeInstruction> Cities_Room_Notify_ContainedThingSpawnedOrDespawned_TranspilerPatch(IEnumerable<CodeInstruction> instructions)
    {
        
        var instructionsList = new List<CodeInstruction>(instructions);
        
        // We need to find the instruction where get_IsDoorway is called on otherRegion
        // Looking at the IL code, this happens at IL_007d after otherRegion is stored in stloc.2

        for (int i = 0; i < instructionsList.Count; i++)
        {
            // Looking for the sequence: ldloc.2 (load otherRegion) followed by callvirt for get_IsDoorway
            if (i + 1 < instructionsList.Count &&
                instructionsList[i].opcode == OpCodes.Ldloc_2 &&
                instructionsList[i + 1].opcode == OpCodes.Callvirt &&
                instructionsList[i + 1].operand is MethodInfo methodInfo &&
                methodInfo.Name == "get_IsDoorway")
            {
                ModLog.Log("Found get_IsDoorway call on otherRegion, inserting null check");

                // Get the instruction where we should jump to if otherRegion is null
                // This should be the instruction after the brtrue.s (IL_0082) which is IL_0091
                var jumpTarget = instructionsList[i + 11]; // This should be the MoveNext instruction

                // Create a new label for our branch instruction
                var label = new Label();
                jumpTarget.labels.Add(label);

                // Insert instructions to check if otherRegion is null before the original ldloc.2
                var newInstructions = new List<CodeInstruction>
                {
                    new(OpCodes.Ldloc_2), // Load otherRegion
                    new(OpCodes.Brfalse, label) // Branch to jumpTarget if null
                };

                instructionsList.InsertRange(i, newInstructions);

                // Skip ahead past the instructions we just processed
                i += newInstructions.Count;
            }

            yield return instructionsList[i];
        }
    }

    // [HarmonyPatch(typeof(GenCity), nameof(GenCity.SpawnInhabitant), [typeof(IntVec3), typeof(Map), typeof(LordJob), typeof(bool), typeof(bool), typeof(PawnKindDef)])]
    // [HarmonyPrefix]
    // public static bool SpawnInhabitantPatch1(Map map)
    // {
    //     return !map.GetCityFaction().IsPlayer;
    // }
    //
    // [HarmonyPatch(typeof(GenCity), nameof(GenCity.SpawnInhabitant), [typeof(IntVec3), typeof(Map), typeof(Lord), typeof(PawnKindDef)])]
    // [HarmonyPrefix]
    // public static bool SpawnInhabitantPatch2(Map map)
    // {
    //     return !map.GetCityFaction().IsPlayer;
    // }
}