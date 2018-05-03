using System.Collections;
using System.Collections.Generic;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Assets.Gamelogic.Core;


namespace Lilsumn.Avalon.World {

    public class SnapshotWorldManager : MonoBehaviour {

        public static Improbable.Collections.Map<int, ChunkSnap> _chunks = new Improbable.Collections.Map<int, ChunkSnap>();

    }

    public struct ChunkSnap {
        public Coord coord;
        public Improbable.Collections.Map<int, TileInfo> tiles;
        public Improbable.Collections.Map<int, ChunkRegion> regions;
        public Vector3 position;

        public ChunkSnap(Coord c, Improbable.Collections.Map<int, TileInfo> t, Improbable.Collections.Map<int, ChunkRegion> r, Vector3 p) {
            coord = c;
            tiles = t;
            regions = r;
            position = p;
        }
    }
}