﻿using System;

namespace CodeBase.DI
{
    public class LifeTimeSelecter : IBindLifeTime
    {
        private DIContainer _container;
        private BindInfo _bindInfo;
        private Type _contractType;

        public LifeTimeSelecter(DIContainer container, BindInfo bindInfo, Type contractType)
        {
            _container = container;
            _bindInfo = bindInfo;
            _contractType = contractType;
        }

        public void AsSingleton()
        {
            SelectLifeTime(ScopeType.Singletone, _bindInfo, new SingletonInstanceProvider(_container, _bindInfo));
        }

        public void AsTransient()
        {
            SelectLifeTime(ScopeType.Transient, _bindInfo, new TransientInstanceProvider(_container, _bindInfo));
        }

        private void SelectLifeTime(ScopeType scopeType, BindInfo bindInfo, IProvider provider)
        {
            if (_bindInfo != null)
            {
                if (_bindInfo.ScopeType != scopeType && _bindInfo.ScopeType == ScopeType.None)
                    _bindInfo.ScopeType = scopeType;

                _container?.FinalizeBind(_bindInfo, new ProviderInfo(provider), _contractType);
            }
        }
    }
}