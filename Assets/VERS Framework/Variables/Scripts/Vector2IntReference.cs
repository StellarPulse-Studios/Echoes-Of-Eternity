using System;
using UnityEngine;

namespace VERS
{
    [Serializable]
    public class Vector2IntReference : Reference<Vector2Int, Vector2IntVariable>
    {
        public Vector2IntReference() { }

        public Vector2IntReference(Vector2Int value) : base(value) { }

        public static implicit operator Vector2IntReference(Vector2Int value)
        {
            return new Vector2IntReference(value);
        }
    }
}
