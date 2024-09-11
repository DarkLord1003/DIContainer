using System;

namespace CodeBase.DI
{
    public class TypeValuePair
    {
        public readonly Type Type;
        public readonly object Value;

        public TypeValuePair(Type type, object value)
        {
            Type = type;
            Value = value;
        }
    }
}