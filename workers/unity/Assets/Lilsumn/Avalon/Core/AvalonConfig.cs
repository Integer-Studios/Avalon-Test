using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lilsumn.Avalon.World;

namespace Lilsumn.Avalon.Core {

    public class AvalonConfig : MonoBehaviour {


        public bool IsClient;

        public Texture2D Heightmap;
        public Texture2D[] Piecemaps;
        public int MaxHeight = 20;

        public Material GhostYesMat;
        public Material GhostNoMat;

        public int InitialVillagers = 4;

        public bool DebugChunks;
        public TileConfigInfo[] Tiles;
        [HideInInspector]
        public Dictionary<int, TileConfigInfo> TileMap = new Dictionary<int, TileConfigInfo>();

        public static AvalonConfig Instance;

        public static State _state = State.SPAWNING;

        public static float _timeSinceLastSpawn = 0f;
        public static int _count;

        private static List<IStartUpListener> _startUpListeners = new List<IStartUpListener>();

        private void Awake() {
            Instance = this;

            foreach (TileConfigInfo i in Tiles) {
                TileMap.Add(i.Id, i);
            }
        }

        private void Update() {
            if (_state == State.SPAWNING) {
                if (_timeSinceLastSpawn > 5f)
                    Initialize();
                else
                    _timeSinceLastSpawn += Time.deltaTime;
            }
        }

        private static void Initialize() {
            if (!Instance.IsClient) {
                Debug.LogWarning("Initializing World...");
                _state = State.INITIALIZING;
                _state = State.READY;
                OnReady();
            } else {
                _state = State.READY;
                OnReady();
            }
        }

        public static void OnReady() {
            foreach (IStartUpListener l in _startUpListeners) {
                l.OnWorkerReady();
            }
            Debug.LogWarning("Ready.");

        }

        public static void OnSpawn() {
            _timeSinceLastSpawn = 0f;
            _count++;
        }

        public static void OnSpawn(IStartUpListener l) {
            _startUpListeners.Add(l);
            OnSpawn();
        }

        public static void OnDestroy(IStartUpListener l) {
            _startUpListeners.Remove(l);
        }

    }

    public enum State {

        SPAWNING,
        INITIALIZING,
        READY

    }

}