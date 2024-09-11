using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.DI
{
    public class DIContainer : IDependencyResolver, IInstantiator
    {
        private readonly Dictionary<BindID, BindInfo> _bindings;
        private readonly Dictionary<BindID, ProviderInfo> _providers;
        private readonly InjectionHandler _injectionHandler;

        public DIContainer()
        {
            _bindings = new Dictionary<BindID, BindInfo>(); 
            _providers = new Dictionary<BindID, ProviderInfo>();
            _injectionHandler = new InjectionHandler(this);
        }

        public Binder<TInterface> Bind<TInterface>()
        {
            BindInfo bindInfo = GetBindInfo(typeof(TInterface));
            return new Binder<TInterface>(this, bindInfo);
        }

        public TInterface Resolve<TInterface>()
        {
            return (TInterface)Resolve(typeof(TInterface)); 
        }

        public void Inject(object instance, InjectionType injectionType = InjectionType.All)
        {
            if (instance == null)
                throw new InvalidOperationException("You must specify a reference to the object!");

            _injectionHandler.Inject(instance, injectionType);
        }

        public void Inject(GameObject gameObject, InjectionType injectionType = InjectionType.All)
        {
            if (gameObject == null)
                throw new InvalidOperationException("To embed dependencies in a game object, you must specify a reference to the object!");

            MonoBehaviour[] behaviours = gameObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in behaviours)
            {
                _injectionHandler.Inject(behaviour);
            }
        }

        public object Resolve(Type contractType)
        {
            if (_providers.TryGetValue(new BindID(contractType), out ProviderInfo providerInfo))
            {
                return providerInfo.Provider.GetInstance();
            }

            throw new InvalidOperationException("This type is not registered in the container!");
        }

        public GameObject Instatiate(GameObject prefab)
        {
            GameObject gameObject = GameObject.Instantiate<GameObject>(prefab);
            Inject(gameObject);

            return gameObject;
        }

        public GameObject Instatiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject gameObject = GameObject.Instantiate<GameObject>(prefab, position, rotation);
            Inject(gameObject);

            return gameObject;
        }

        internal void FinalizeBind(BindInfo bindInfo, ProviderInfo providerInfo, Type contractType)
        {
            BindID bindID = new BindID(contractType);
            if (!_bindings.ContainsKey(bindID))
            {
                _bindings[bindID] = bindInfo;
                _providers[bindID] = providerInfo;
            }
        } 
        
        internal BindInfo GetBindInfo(Type contractType)
        {
            if (_bindings.TryGetValue(new BindID(contractType), out var bindInfo))
                return bindInfo;

            return null;
        }
    }
}
