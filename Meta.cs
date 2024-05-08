using System;
using Assets.Metater.MetaUtils;
using UnityEngine;

namespace Assets.Metater
{
    public static class Meta
    {
        public static double Time => UnityEngine.Time.timeAsDouble;
        public static double Realtime => UnityEngine.Time.unscaledTimeAsDouble;
        public static double FixedTime => UnityEngine.Time.fixedTimeAsDouble;
        public static double FixedRealtime => UnityEngine.Time.fixedUnscaledTimeAsDouble;

        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new();
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new();
        public static readonly WaitForSeconds WaitForTenthSecond = new(0.1f);

        /// <summary>
        /// UNITY_ASSERTIONS is the preprocessor directive equivalent of this
        /// Enabled in the editor and in development builds and disabled other times
        /// </summary>
        public static bool IsDebugBuild => Debug.isDebugBuild;

        public static float GetSinT(float periodSeconds, MetaInstant instant)
        {
            float sin = (float)Math.Sin(2 * Math.PI * instant.TimeSeconds / periodSeconds);
            return (sin + 1) / 2f;
        }

        public static float GetCosT(float periodSeconds, MetaInstant instant)
        {
            float cos = (float)Math.Cos(2 * Math.PI * instant.TimeSeconds / periodSeconds);
            return (cos + 1) / 2f;
        }

        public static int GetLayerIndex(LayerMask layerMask)
        {
            return (int)Math.Log(layerMask.value, 2);
        }
    }
}