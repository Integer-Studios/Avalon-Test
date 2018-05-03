using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Improbable;
using Improbable.Entity.Component;
using Improbable.Unity.Core;

namespace Lilsumn.Avalon.World {

    public class WorldManager {

        public static int ChunkSize = 16;
        public static int TileSize = 4;
        public static int TileHeight = 2;
        public static int WorldSize = 8;

        private static Dictionary<int, ChunkVisualizer> _chunks = new Dictionary<int, ChunkVisualizer>();
        private static Dictionary<int, RegionSetVisualizer> _regionSets = new Dictionary<int, RegionSetVisualizer>();

        //// Conversions

        public static Vector3 Tile2WorldNoHeight(Coord tilePos) {
            return new Vector3(tilePos.x * TileSize, 0, tilePos.z * TileSize);
        }

        public static Coord Chunk2Tile(Coord chunkPos) {
            return new Coord(chunkPos.x * ChunkSize, chunkPos.z * ChunkSize);
        }

        public static Vector3 GetChunkCenter(Coord chunkPos) {
            int x = chunkPos.x;
            int z = chunkPos.z;
            x *= ChunkSize;
            z *= ChunkSize;
            x += ChunkSize / 2;
            z += ChunkSize / 2;
            return Tile2WorldNoHeight(new Coord(x, z));
        }

       

        //// Hash Functions


        public static int HashCoord(Coord c) {
            return Hash(c.x, c.z);
        }

        public static Coord UnhashCoord(int c) {
            int x, z;
            Unhash(c, out x, out z);
            return new Coord(x, z);
        }

        private static int Hash(int k1, int k2) {
            PreprocessHashInt(ref k1);
            PreprocessHashInt(ref k2);
            int a = (k1 + k2) * (k1 + k2 + 1);
            a /= 2;
            a += k2;
            return a;
        }

        private static void PreprocessHashInt(ref int i) {
            if (i >= 0)
                i *= 2;
            else
                i = (-2 * i) - 1;
        }

        private static void Unhash(int z, out int k1, out int k2) {
            float w = (8f * (float)z) + 1f;
            w = Mathf.Sqrt(w);
            w -= 1f;
            w /= 2f;
            int wi = Mathf.FloorToInt(w);
            int t = (wi * wi) + wi;
            t /= 2;
            k2 = z - t;
            k1 = wi - k2;
            PostprocessUnhashInt(ref k2);
            PostprocessUnhashInt(ref k1);
        }

        private static void PostprocessUnhashInt(ref int i) {
            if (i % 2 == 0)
                i /= 2;
            else
                i = (i + 1) / (-2);
        }

    }

    // Types

    public struct Coord {
        public int x;
        public int z;
        public Coord(int xx, int zz) {
            x = xx;
            z = zz;
        }
        public bool Same(Coord c) {
            return x == c.x && z == c.z;
        }
        public string Print() {
            return x + "," + z;
        }
    }

    [System.Serializable]
    public struct TileConfigInfo {
        public int Id;
        public Color TopColor;
        public Color SideColor;
    }

}