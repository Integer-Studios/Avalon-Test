﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Improbable;
using Improbable.Core;
using Improbable.Unity.Visualizer;

namespace Assets.Gamelogic.Core {

    public class PositionController : MonoBehaviour {

        [Require] private Position.Writer positionWriter;

        void OnEnable() {
            transform.position = positionWriter.Data.coords.ToVector3();
            StartCoroutine("UpdatePosition");
        }

        IEnumerator UpdatePosition() {
            while (enabled) {
                if (transform.position != positionWriter.Data.coords.ToVector3())
                    positionWriter.Send(new Position.Update().SetCoords(transform.position.ToCoordinates()));
                yield return new WaitForSeconds(1 / 9);

            }
        }

    }

    public static class Vector3Extensions {
        public static Coordinates ToCoordinates(this Vector3 vector3) {
            return new Coordinates(vector3.x, vector3.y, vector3.z);
        }

        public static Vector3 ToVector3(this Coordinates coord) {
            return new Vector3((float)coord.x, (float)coord.y, (float)coord.z);
        }
    }

}