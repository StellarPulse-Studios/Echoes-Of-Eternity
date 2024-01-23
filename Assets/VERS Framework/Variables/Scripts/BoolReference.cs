using System;

namespace VERS
{
    [Serializable]
    public class BoolReference : Reference<bool, BoolVariable>
    {
        public BoolReference() { }

        public BoolReference(bool value) : base(value) { }

        public static implicit operator BoolReference(bool value)
        {
            return new BoolReference(value);
        }
    }
}
