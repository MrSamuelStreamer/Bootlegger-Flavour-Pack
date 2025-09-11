using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace MSSBL;

public class WorldDrawLayer_Tunnels: WorldDrawLayer_Paths
{
    public override bool VisibleWhenLayerNotSelected => false;

    public override bool VisibleInBackground => false;

    public override IEnumerable Regenerate()
    {
        foreach (object obj in base.Regenerate())
        {
            yield return obj;
        }
        
        IEnumerator enumerator = null;
        LayerSubMesh subMesh = GetSubMesh(WorldMaterials.Roads);
        List<RoadWorldLayerDef> roadLayerDefs = DefDatabase<RoadWorldLayerDef>.AllDefs.OrderBy((RoadWorldLayerDef def) => def.order).ToList<RoadWorldLayerDef>();
        
        int num;
        for (int i = 0; i < planetLayer.TilesCount; i = num)
        {
            if (i % 1000 == 0)
            {
                yield return null;
            }
            if (subMesh.verts.Count > 60000)
            {
                subMesh = GetSubMesh(WorldMaterials.Roads);
            }
            
            SurfaceTile surfaceTile = (SurfaceTile)planetLayer[i];
            if (!surfaceTile.WaterCovered)
            {
                List<OutputDirection> list = new List<OutputDirection>();
                
                if (surfaceTile.potentialRoads != null)
                {
                    bool flag = true;
                    for (int j = 0; j < surfaceTile.potentialRoads.Count - 1; j++)
                    {
                        if (surfaceTile.potentialRoads[j].road.worldTransitionGroup != surfaceTile.potentialRoads[j + 1].road.worldTransitionGroup)
                        {
                            flag = false;
                        }
                    }
                    for (int k = 0; k < roadLayerDefs.Count; k++)
                    {
                        bool flag2 = false;
                        list.Clear();
                        for (int l = 0; l < surfaceTile.potentialRoads.Count; l++)
                        {
                            RoadDef road = surfaceTile.potentialRoads[l].road;
                            float layerWidth = road.GetLayerWidth(roadLayerDefs[k]);
                            if (layerWidth > 0f)
                            {
                                flag2 = true;
                            }
                            list.Add(new OutputDirection
                            {
                                neighbor = surfaceTile.potentialRoads[l].neighbor,
                                width = layerWidth,
                                distortionFrequency = road.distortionFrequency,
                                distortionIntensity = road.distortionIntensity
                            });
                        }
                        if (flag2)
                        {
                            GeneratePaths(subMesh, new PlanetTile(i, planetLayer), list, roadLayerDefs[k].color, flag);
                        }
                    }
                }
            }
            num = i + 1;
        }
        FinalizeMesh(MeshParts.All);
    }

    public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
    {
        throw new System.NotImplementedException();
    }
}