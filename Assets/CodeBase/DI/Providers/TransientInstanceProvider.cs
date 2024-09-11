namespace CodeBase.DI
{
    public class TransientInstanceProvider : InstanceProviderBase
    {
        public TransientInstanceProvider(DIContainer container, BindInfo bindInfo) : base(container, bindInfo)
        {
        }

        public override object GetInstance()
        {
            if (bindInfo != null)
            {
                return base.GetInstance();
            }

            return null;
        }
    }
}
