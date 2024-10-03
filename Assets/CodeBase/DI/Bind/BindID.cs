using System;

namespace CodeBase.DI
{
    public struct BindID : IEquatable<BindID>
    {
        private Type _type;

        public BindID(Type type)
        {
            _type = type;
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
            unchecked
            {
                int hash = 17;
                hash = hash * 29 + _type.GetHashCode();
                return hash;
            }
        }

        public bool Equals(BindID other)
        {
            return other == this;
        }

        public static bool operator == (BindID left, BindID right)
        {
            return left._type == right._type;
        }

        public static bool operator != (BindID left, BindID right)
        {
            return !left.Equals(right);
        }
    }
}
