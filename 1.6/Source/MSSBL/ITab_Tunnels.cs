using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace MSSBL;

// ReSharper disable once InconsistentNaming
public class ITab_Tunnels: ITab
{
    private static readonly Vector2 WinSize = new(420f, 480f);
    
    public ITab_Tunnels()
    {
        size = WinSize;
        labelKey = "MSSBLTabTunnels";

        if(_thread is not { IsAlive: true }){
            _thread = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                CalculatePaths();
            });
            
            _thread.Start();
        }
    }

    private static Thread _thread;

    private static readonly Dictionary<Settlement, PathDetails> CachedPaths = new();

    public Building_TunnelEntrance TunnelEntrance => SelThing as Building_TunnelEntrance;


    private class PathDetails()
    {
        public List<PlanetTile> Nodes = [];
        public bool Processing = true;

        public int Count => Nodes.Count;

        public override string ToString()
        {
            return $"PathDetails: Processing: {Processing}, NodeCount: {Count}";
        }
    }

    private struct PathCalc
    {
        public Settlement Settlement;
        public PlanetTile Tile;
    }

    private static readonly ConcurrentQueue<PathCalc> QueuedPathCalcs = new();

    private static void CalculatePaths()
    {
        while (true)
        {
            while (QueuedPathCalcs.TryDequeue(out PathCalc pathCalc))
            {
                var path = pathCalc.Settlement.Tile.Layer.Pather.FindPath(pathCalc.Tile, pathCalc.Settlement.Tile, null);

                var pathDetails = CachedPaths[pathCalc.Settlement];
                pathDetails.Nodes = path.NodesReversed.ToList();
                pathDetails.Processing = false;
                CachedPaths[pathCalc.Settlement] = pathDetails;
                path.ReleaseToPool();
            }
            Thread.Sleep(10);
        }
    }

    private PathDetails TryGetCachedPath(Settlement settlement)
    {
        if (CachedPaths.TryGetValue(settlement, out var path)) return path;
        QueuedPathCalcs.Enqueue(new PathCalc { Settlement = settlement, Tile = SelThing.Tile });
        CachedPaths[settlement] = new PathDetails() { Processing = true };

        return CachedPaths[settlement];
    }
    
    private float ScrollViewHeight = 0;
    public Vector2 scrollPosition = Vector2.zero;
    
    protected override void FillTab()
    {
        var rect = new Rect(0, 0, WinSize.x + 32, WinSize.y);
        var tab = new Listing_Standard
        {
            maxOneColumn = true
        };
        Rect contentScrollContainerRect = new(
            rect.xMin,
            rect.yMin,
            rect.width,
            Mathf.Max(ScrollViewHeight, rect.height)
        );
        scrollPosition = GUI.BeginScrollView(rect, scrollPosition, contentScrollContainerRect);
        
        tab.Begin(contentScrollContainerRect.ContractedBy(2f));

        try
        {
            tab.GapLine();
            foreach (var settlement in Find.WorldObjects.Settlements.Where(s =>
                         s.Faction.def != FactionDefOf.Empire && s.Tile != SelThing.Tile &&
                         s.Tile.Layer == SelThing.Tile.Layer))
            {
                var distance = settlement.Tile.Layer.ApproxDistanceInTiles(settlement.Tile, SelThing.Tile);

                if (TunnelEntrance.CurrentBill == null)
                {
                    if (tab.ButtonTextLabeledPct(settlement.LabelCap, "Start Digging", 0.5f,
                            labelIcon: settlement.def.ExpandingIconTexture))
                    {
                        TunnelEntrance.CurrentBill = new TunnelBill(settlement, TunnelEntrance.Map, distance*1000);
                    }
                }else if (TunnelEntrance.CurrentBill.Settlement == settlement)
                {
                    if (tab.ButtonTextLabeledPct(settlement.LabelCap, "Cancel Digging", 0.5f,
                            labelIcon: settlement.def.ExpandingIconTexture))
                    {
                        TunnelEntrance.CurrentBill = null;
                    }
                }
                tab.LabelDouble("Path length", $"{distance}");

                tab.GapLine();
            }
        }
        finally
        {
            GUI.EndScrollView();
            ScrollViewHeight = tab.CurHeight;
            tab.End();
        }

    }
}