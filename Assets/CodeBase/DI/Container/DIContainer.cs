using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.DI
{
    public class DIContainer : IDependencyResolver, IInstantiator
    {
        private readonly Dictionary<BindID, BindInfo> _bindings;
        private readonly Dictionary<BindID, ProviderInfo> _providers;

        private readonly HashSet<Type> _resolvingTypes;
        private readonly InjectionHandler _injectionHandler;
        private readonly DIContainer _parentContainer;

        public DIContainer(DIContainer parentContainer)
        {
            _bindings = new Dictionary<BindID, BindInfo>(); 
            _providers = new Dictionary<BindID, ProviderInfo>();
            _resolvingTypes = new HashSet<Type>();
            _injectionHandler = new InjectionHandler(this);
        }

        public DIContainer() : this(null)
        {

        }

        public DIContainer CreateSubContainer()
        {
            return new DIContainer(this);
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
            if (_resolvingTypes.Contains(contractType))
                throw new InvalidOperationException($"Circular dependency detected for type - {contractType.FullName}");

            _resolvingTypes.Add(contractType);

            try
            {
                if (_providers.TryGetValue(new BindID(contractType), out ProviderInfo providerInfo))
                {
                    return providerInfo.Provider.GetInstance();
                }

                if (_parentContainer != null)
                {
                    return _parentContainer.Resolve(contractType);
                }

                throw new InvalidOperationException($"This type - {contractType.FullName} - is not registered in the container!");
            }
            finally
            {
                _resolvingTypes.Remove(contractType);
            }
        }

        public TImplementation InstantiatePrefabForComponent<TImplementation>(GameObject prefab, Vector3 position, Quaternion rotation, bool searchInChildren = false) 
            where TImplementation : Component
        {
            GameObject gameObject = InstantiatePrefab(prefab, position, rotation);

            TImplementation component = null;
            if(searchInChildren)
                component = gameObject.GetComponentInChildren<TImplementation>();
            else
                component = gameObject.GetComponent<TImplementation>();

            if (component == null)
                throw new InvalidOperationException($"Component of type {typeof(TImplementation).FullName} not found on the instantiated object.");

            return component;
        }

        public GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
                throw new InvalidOperationException("It is necessary to pass the prefab to create a game object!");

            GameObject gameObject = GameObject.Instantiate(prefab, position, rotation);
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

        internal void RegisterNonLazy(Type contractType)
        {
            if (_providers.TryGetValue(new BindID(contractType), out ProviderInfo providerInfo))
            {
                _providers[new BindID(contractType)].Provider.GetInstance();
            }
        }
    }
}
