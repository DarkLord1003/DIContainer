using System;

namespace CodeBase.DI
{
    public interface IInjection
    {
        void Inject(object target);
    }

    [Flags]
    public enum InjectionType
    {
        Field = 0,
        Property = 1,
        Method = 2,
        All = Field | Property | Method
    }
}
