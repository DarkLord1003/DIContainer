namespace CodeBase.DI
{
    public interface IBindLifeTime
    {
        void AsTransient();
        void AsSingleton();
    }
}