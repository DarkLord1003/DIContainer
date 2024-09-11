using System.Linq;

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
                if (bindInfo.Instance != null)
                {
                    return bindInfo.Instance;
                }
                else
                {
                    bindInfo.Instance = CreateInstance(bindInfo);
                    return bindInfo.Instance;
                }
            }

            return null;
        }
    }
}
