
namespace CodeBase.DI
{
    public interface IBindFromInstance<TInterface>
    {
        IBindSingleton FromInstance<TImplementation>(object instance) where TImplementation : class, TInterface;
    }
}
