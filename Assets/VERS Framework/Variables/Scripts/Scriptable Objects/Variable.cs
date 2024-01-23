using UnityEngine;

namespace VERS
{
    public abstract class Variable<T> : ScriptableObject
    {
        public T Value;
    }
}
