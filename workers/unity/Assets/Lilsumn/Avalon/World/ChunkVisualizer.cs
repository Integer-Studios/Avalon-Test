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

    public class ChunkVisualizer : MonoBehaviour {

        [Require]
        public Chunk.Reader ChunkReader;
        [Require]
        public RegionSet.Reader RegionReader;
        public static int count;

        public void OnEnable() {
            count++;
            Debug.LogWarning("Chunk Visualzier Enable Count: " + count);


        }


    }

}