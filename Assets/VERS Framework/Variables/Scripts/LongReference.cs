using System;

namespace VERS
{
    [Serializable]
    public class LongReference : Reference<long, LongVariable>
    {
        public LongReference() { }

        public LongReference(long value) : base(value) { }

        public static implicit operator LongReference(long value)
        {
            return new LongReference(value);
        }
    }
}
