using System;

namespace CodeBase.DI
{
    public class Binder<TInterface> : IBindTo<TInterface>, IBindToWithArguments<TInterface>, IBindFromFactory<TInterface>
    {
        private DIContainer _container;
        private BindInfo _bindInfo;

        public Binder(DIContainer container, BindInfo bindInfo)
        {
            _container = container;
            _bindInfo = bindInfo;
        }

        public IBindLifeTime FormFactory<TImplementation>(Func<TImplementation> factory) where TImplementation : class, TInterface
        {
            CreateOrSetBindInfo(typeof(TImplementation));
            _bindInfo.Instance = factory?.Invoke();

            return new LifeTimeSelecter(_container, _bindInfo, typeof(TInterface));
        }

        public IBindLifeTime To<TImplementation>() where TImplementation : class, TInterface
        {
            CreateOrSetBindInfo(typeof(TImplementation));
            return new LifeTimeSelecter(_container, _bindInfo, typeof(TInterface));
        }

        public IBindLifeTime ToWithArguments<TImplementation>(params object[] args) where TImplementation : class, TInterface
        {
            CreateOrSetBindInfo(typeof(TImplementation));
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    _bindInfo.Arguments.Add(new TypeValuePair(args[i].GetType(), args[i]));
                }
            }

            return new LifeTimeSelecter(_container, _bindInfo, typeof(TInterface));
        }

        private void CreateOrSetBindInfo(Type to)
        {
            if (_bindInfo == null)
                _bindInfo = new BindInfo();

            Type contractType = typeof(TInterface);

            if (to.IsAbstract || to.IsInterface)
                throw new InvalidOperationException("This type cannot be registered because it is an abstraction or interface!");

            if (_bindInfo.ScopeType == ScopeType.Singletone)
                throw new InvalidOperationException("This type has already been registered!");

            _bindInfo.Reset();
            _bindInfo.ContractTypes.Add(contractType);
            _bindInfo.ToTypes.Add(to);
        }
    }
}
