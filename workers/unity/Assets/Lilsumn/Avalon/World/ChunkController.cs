using System.Collections;
using System.Collections.Generic;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Assets.Gamelogic.Core;
using Lilsumn.Avalon.Core;
using Improbable;
using Improbable.Entity.Component;


namespace Lilsumn.Avalon.World {

    public class ChunkController : MonoBehaviour {

        [Require]
        public Chunk.Writer chunkWriter;
        [Require]
        public RegionSet.Writer regionWriter;
       
        public static int count;

        public void OnEnable() {
            count++;
            Debug.LogWarning("Chunk Controller Enable Count: " + count);
            AvalonConfig.OnSpawn();
        }

        // Update is called once per frame
        void Update() {

        }
    }

}
