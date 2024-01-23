using System;
using UnityEngine;

namespace VERS
{
    [Serializable]
    public class Vector3IntReference : Reference<Vector3Int, Vector3IntVariable>
    {
        public Vector3IntReference() { }

        public Vector3IntReference(Vector3Int value) : base(value) { }

        public static implicit operator Vector3IntReference(Vector3Int value)
        {
            return new Vector3IntReference(value);
        }
    }
}
