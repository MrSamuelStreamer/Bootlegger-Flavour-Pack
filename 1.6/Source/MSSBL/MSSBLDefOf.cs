using RimWorld;

namespace MSSBL;

[DefOf]
public static class MSSBLDefOf
{
    public static BiomeDef MSSBL_City_PlayerHome;

    static MSSBLDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(MSSBLDefOf));
}
