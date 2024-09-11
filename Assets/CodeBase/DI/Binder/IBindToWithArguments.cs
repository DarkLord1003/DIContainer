namespace CodeBase.DI
{
    public interface IBindToWithArguments<TInterface>
    {
        IBindLifeTime ToWithArguments<TImplementation>(params object[] args) where TImplementation : class, TInterface;
    }
}
