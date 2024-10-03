
namespace CodeBase.DI
{
    public class SingletonInstanceProvider : InstanceProviderBase
    {
        public SingletonInstanceProvider(DIContainer container, BindInfo bindInfo) : base(container, bindInfo)
        {
        }

        public override object GetInstance()
        {
            if (bindInfo != null)
            {
                if (bindInfo.Instance == null)
                {
                    bindInfo.Instance = bindInfo.Factory == null ? CreateInstance(bindInfo) : bindInfo.Factory;
                }

                return bindInfo.Instance;
            }

            return null;
        }
    }
}
