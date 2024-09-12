
namespace CodeBase.DI
{
    public interface IBindSingleton
    {
        IBindNonLazy AsSingleton();
    }
}