using System;

namespace CodeBase.DI
{
    public interface IDependencyResolver
    {
        object Resolve(Type type);
    }
}
