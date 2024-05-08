using System;
using Random = UnityEngine.Random;

namespace Assets.Metater.MetaUtils
{
    [Serializable]
    public struct MetaRandomFloat
    {
        public float min;
        public float max;

        public readonly float Value => Random.Range(min, max);
    }
}
