using HarmonyLib;
using UnityEngine;
using Verse;

namespace MSSBL;

[StaticConstructorOnStartup]
public class MSSBLMod : Mod
{
    public static Settings settings;
    public MSSBLMod(ModContentPack content)
        : base(content)
    {
        ModLog.Log("Loading the Mr Samuel Streamer Bootleg Flavour Pack");
        
        // initialize settings
        settings = GetSettings<Settings>();
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("MrSamuelStreamer.rimworld.MSSBL.main");
        harmony.PatchAll();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        settings.DoWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "MSSBL_SettingsCategory".Translate();
    }
}
