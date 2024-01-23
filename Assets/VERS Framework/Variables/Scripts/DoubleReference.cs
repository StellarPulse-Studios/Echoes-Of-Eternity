using System;

namespace VERS
{
    [Serializable]
    public class DoubleReference : Reference<double, DoubleVariable>
    {
        public DoubleReference() { }

        public DoubleReference(double value) : base(value) { }

        public static implicit operator DoubleReference(double value)
        {
            return new DoubleReference(value);
        }
    }
}
