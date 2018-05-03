using Assets.Gamelogic.Core;
using Improbable;
using Improbable.Worker;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Assets.Gamelogic.EntityTemplates;
using Lilsumn.Avalon.Core;
using Lilsumn.Avalon.World;

namespace Assets.Editor {
    public class SnapshotMenu : MonoBehaviour {
        [MenuItem("Improbable/Snapshots/Generate Default Snapshot")]
        [UsedImplicitly]
        private static void GenerateDefaultSnapshot() {
            var snapshotEntities = new Dictionary<EntityId, Entity>();

            AvalonConfig conf = FindObjectOfType<AvalonConfig>();
            Color[] heights = conf.Heightmap.GetPixels();
            List<Color[]> pieces = new List<Color[]>();
            foreach (Texture2D piecemap in conf.Piecemaps) {
                if (piecemap == null)
                    pieces.Add(null);
                else
                    pieces.Add(piecemap.GetPixels());
            }
            int width = conf.Heightmap.width;

            // Add entity data to the snapshot
            var currentEntityId = 1;
            Improbable.Collections.Map<int, ChunkSnap> chunks = new Improbable.Collections.Map<int, ChunkSnap>();

            for (int cz = -1 * (WorldManager.WorldSize / 2); cz < WorldManager.WorldSize / 2; cz++) {
                for (int cx = -1 * (WorldManager.WorldSize / 2); cx < WorldManager.WorldSize / 2; cx++) {
                    Coord chunkPos = new Coord(cx, cz);
                    Improbable.Collections.Map<int, TileInfo> chunkData = new Improbable.Collections.Map<int, TileInfo>();
                    for (int z = 0; z < WorldManager.ChunkSize; z++) {
                        for (int x = 0; x < WorldManager.ChunkSize; x++) {
                            Coord tileOffset = WorldManager.Chunk2Tile(chunkPos);
                            int xcomp = (x + tileOffset.x + ((WorldManager.WorldSize / 2) * WorldManager.ChunkSize));
                            int zcomp = (z + tileOffset.z + ((WorldManager.WorldSize / 2) * WorldManager.ChunkSize));
                            int sample = xcomp + (zcomp * width);
                            int height = (int)(heights[sample].r * conf.MaxHeight);
                            Coord tile = new Coord(x + tileOffset.x, z + tileOffset.z);
                            chunkData.Add(x + (WorldManager.ChunkSize * z), new TileInfo(0, height, -1, -1, new Improbable.Collections.Option<EntityId>()));

                        }
                    }

                    Improbable.Collections.Map<int, ChunkRegion> r = new Improbable.Collections.Map<int, ChunkRegion>();

                    chunks[WorldManager.HashCoord(chunkPos)] = new ChunkSnap(chunkPos, chunkData, r, WorldManager.GetChunkCenter(chunkPos));


                }
            }

            SnapshotWorldManager._chunks = chunks;

            for (int cz = -1 * (WorldManager.WorldSize / 2); cz < WorldManager.WorldSize / 2; cz++) {
                for (int cx = -1 * (WorldManager.WorldSize / 2); cx < WorldManager.WorldSize / 2; cx++) {
                    Coord chunkPos = new Coord(cx, cz);
                    ChunkSnap c = chunks[WorldManager.HashCoord(chunkPos)];

                    //SnapshotWorldManager.ConnectRegions(cx, cz, ref c.tiles, ref c.regions);


                    snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateChunkTemplate(new Chunk.Data(c.coord.x, c.coord.z, c.tiles), new RegionSet.Data(c.coord.x, c.coord.z, c.regions), c.position));

                }
            }


            //int xcompo = (((WorldManager.WorldSize / 2) * WorldManager.ChunkSize));
            //int zcompo = (((WorldManager.WorldSize / 2) * WorldManager.ChunkSize));
            //int y = (int)(heights[xcompo + (zcompo * width)].r * conf.MaxHeight);
            //Debug.Log(y*WorldManager.TileHeight);
            // snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateCharacterEntityTemplate(new Lilsumn.Avalon.Villager.Villager.Data(0, 0, Bytes.CopyOf(new byte[0])), new Vector3(WorldManager.TileSize/2 - (5 * WorldManager.TileSize), 0, WorldManager.TileSize/2)));

            SaveSnapshot(snapshotEntities);
        }

        private static void SaveSnapshot(IDictionary<EntityId, Entity> snapshotEntities) {
            File.Delete(SimulationSettings.DefaultSnapshotPath);
            using (SnapshotOutputStream stream = new SnapshotOutputStream(SimulationSettings.DefaultSnapshotPath)) {
                foreach (var kvp in snapshotEntities) {
                    var error = stream.WriteEntity(kvp.Key, kvp.Value);
                    if (error.HasValue) {
                        Debug.LogErrorFormat("Failed to generate initial world snapshot: {0}", error.Value);
                        return;
                    }
                }
            }

            Debug.LogFormat("Successfully generated initial world snapshot at {0}", SimulationSettings.DefaultSnapshotPath);
        }


    }
}
