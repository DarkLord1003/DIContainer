using System;

namespace CodeBase.DI
{
    public struct BindID : IEquatable<BindID>
    {
        private Type Type;

        public BindID(Type type)
        {
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) 
                return false;

            if (obj is BindID bindID)
                return bindID == this;

            return false;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public bool Equals(BindID other)
        {
            return other == this;
        }

        public static bool operator == (BindID left, BindID right)
        {
            return left.Type == right.Type;
        }

        public static bool operator != (BindID left, BindID right)
        {
            return !left.Equals(right);
        }
    }
}
