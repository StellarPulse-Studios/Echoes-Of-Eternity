using System;
using UnityEngine;

namespace VERS
{
    [Serializable]
    public class Vector2Reference : Reference<Vector2, Vector2Variable>
    {
        public Vector2Reference() { }

        public Vector2Reference(Vector2 value) : base(value) { }

        public static implicit operator Vector2Reference(Vector2 value)
        {
            return new Vector2Reference(value);
        }
    }
}
