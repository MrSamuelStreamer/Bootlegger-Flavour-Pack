using HarmonyLib;
using Verse;

namespace MSSBL;

[StaticConstructorOnStartup]
public class MSSBLMod : Mod
{
    public MSSBLMod(ModContentPack content)
        : base(content)
    {
        ModLog.Log("Loading the Mr Samuel Streamer Flavour Pack");

    }
}
