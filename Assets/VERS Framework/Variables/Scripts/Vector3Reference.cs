using System;
using UnityEngine;

namespace VERS
{
    [Serializable]
    public class Vector3Reference : Reference<Vector3, Vector3Variable>
    {
        public Vector3Reference() { }

        public Vector3Reference(Vector3 value) : base(value) { }

        public static implicit operator Vector3Reference(Vector3 value)
        {
            return new Vector3Reference(value);
        }
    }
}
