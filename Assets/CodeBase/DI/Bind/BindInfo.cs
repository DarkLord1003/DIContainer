using System;
using System.Collections.Generic;

namespace CodeBase.DI
{
    public class BindInfo
    {
        public readonly List<Type> ContractTypes;
        public readonly List<Type> ToTypes;
        public readonly List<TypeValuePair> Arguments;

        public object Instance { get; set; }
        public ScopeType ScopeType { get; set; }

        public BindInfo()
        {
            ContractTypes = new List<Type>();
            ToTypes = new List<Type>();
            Arguments = new List<TypeValuePair>();
        }

        public void Reset()
        {
            ContractTypes.Clear();
            ToTypes.Clear();
            Arguments.Clear();

            Instance = null;
            ScopeType = ScopeType.None;
        }
    }

    public enum ScopeType
    {
        None,
        Transient,
        Singletone,
    }
}
