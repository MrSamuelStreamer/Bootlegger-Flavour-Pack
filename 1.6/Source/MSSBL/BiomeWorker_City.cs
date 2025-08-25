using RimWorld;
using RimWorld.Planet;

namespace MSSBL;

public class BiomeWorker_City: BiomeWorker
{
    public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
    {
        if (tile.WaterCovered)
            return -100f;
        return 100f;
    }
}