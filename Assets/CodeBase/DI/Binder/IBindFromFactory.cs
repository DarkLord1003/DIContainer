using System;

namespace CodeBase.DI
{
    public interface IBindFromFactory<TInterface>
    {
        IBindLifeTime FromFactory<TImplementation>(Func<object> factory) where TImplementation : class, TInterface;
    }
}
