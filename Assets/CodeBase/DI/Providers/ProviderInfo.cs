
namespace CodeBase.DI
{
    public class ProviderInfo
    {
        public readonly IProvider Provider;

        public ProviderInfo(IProvider provider)
        {
            Provider = provider;
        }
    }
}
