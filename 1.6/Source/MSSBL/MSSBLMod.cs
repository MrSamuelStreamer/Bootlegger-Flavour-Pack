using HarmonyLib;
using Verse;

namespace MSSBL;

[StaticConstructorOnStartup]
public class MSSBLMod : Mod
{
    public MSSBLMod(ModContentPack content)
        : base(content)
    {
        ModLog.Log("Loading the Mr Samuel Streamer Bootleg Flavour Pack");
        
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("MrSamuelStreamer.rimworld.MSSBL.main");
        harmony.PatchAll();
    }
}
