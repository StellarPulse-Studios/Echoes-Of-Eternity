using System;

namespace VERS
{
    [Serializable]
    public class IntReference : Reference<int, IntVariable>
    {
        public IntReference() { }

        public IntReference(int value) : base(value) { }

        public static implicit operator IntReference(int value)
        {
            return new IntReference(value);
        }
    }
}
