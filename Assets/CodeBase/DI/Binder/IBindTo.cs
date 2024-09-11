namespace CodeBase.DI
{
    public interface IBindTo<TInterface>
    {
        IBindLifeTime To<TImplementation>() where TImplementation : class, TInterface;
    }
}
