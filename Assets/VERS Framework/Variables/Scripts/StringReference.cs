using System;

namespace VERS
{
    [Serializable]
    public class StringReference : Reference<string, StringVariable>
    {
        public StringReference() { }

        public StringReference(string value) : base(value) { }

        public static implicit operator StringReference(string value)
        {
            return new StringReference(value);
        }
    }
}
