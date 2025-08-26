using RimWorld;
using RimWorld.Planet;
using Verse;

namespace MSSBL;

public class BiomeWorker_City: BiomeWorker
{
    public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
    {
        if (tile.WaterCovered)
            return -100f;
        return 60f + Rand.Range(-4f, 4f);
    }
}