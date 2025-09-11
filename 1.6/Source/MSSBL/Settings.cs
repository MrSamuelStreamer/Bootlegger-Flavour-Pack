using UnityEngine;
using Verse;

namespace MSSBL;


public class Settings : ModSettings
{
    public float chanceToRaid = 0.5f;
    public string chanceToRaid_buffer;

    public void DoWindowContents(Rect wrect)
    {
        Listing_Standard options = new();
        options.Begin(wrect);

        options.Label("MSSBL_Setting_ChanceForRaid".Translate());
        options.TextFieldNumeric(ref chanceToRaid, ref chanceToRaid_buffer, 0f, 1f);
        options.Gap();

        options.End();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref chanceToRaid, "chanceToRaid", 0.5f);
    }
}
