using System;

namespace VERS
{
    [Serializable]
    public class CharReference : Reference<char, CharVariable>
    {
        public CharReference() { }

        public CharReference(char value) : base(value) { }

        public static implicit operator CharReference(char value)
        {
            return new CharReference(value);
        }
    }
}
