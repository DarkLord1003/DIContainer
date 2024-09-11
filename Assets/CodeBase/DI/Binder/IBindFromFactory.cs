using System;

namespace CodeBase.DI
{
    public interface IBindFromFactory<TInterface>
    {
        IBindLifeTime FormFactory<TImplementation>(Func<TImplementation> factory) where TImplementation : class, TInterface;
    }
}
