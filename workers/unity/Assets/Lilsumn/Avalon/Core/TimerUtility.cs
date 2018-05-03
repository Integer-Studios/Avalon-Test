using System;
using System.Collections;
using UnityEngine;

namespace Lilsumn.Avalon.Core
{
    public static class TimerUtility
    {
        public static IEnumerator WaitAndPerform(float bufferTime, Action action)
        {
            yield return new WaitForSeconds(bufferTime);
            action();
        }

        public static IEnumerator CallRepeatedly(float interval, Action action)
        {
            while (true)
            {
                yield return new WaitForSeconds(interval);
                action();
            }
        }
    }
}