using UnityEngine;

namespace CodeBase.DI
{
    public class GameObjectInjectionHandler
    {
        private readonly InjectionIntoGameObjectField _injectionIntoGameObjectField;
        private readonly InjectionIntoGameObjectMethod _injectionIntoGameObjectMethod;
        private readonly InjectionIntoGameObjectProperty _injectionIntoGameObjectProperty;

        public GameObjectInjectionHandler(IDependencyResolver resolver)
        {
            _injectionIntoGameObjectField = new InjectionIntoGameObjectField(resolver);
            _injectionIntoGameObjectMethod = new InjectionIntoGameObjectMethod(resolver);
            _injectionIntoGameObjectProperty = new InjectionIntoGameObjectProperty(resolver);
        }

        public void Inject(MonoBehaviour target, InjectionType injectionType = InjectionType.All, ComponentCheckType componentCheckType = ComponentCheckType.All)
        {
            if (injectionType == InjectionType.Field)
                _injectionIntoGameObjectField.Inject(target, componentCheckType);

            if (injectionType == InjectionType.Method)
                _injectionIntoGameObjectMethod.Inject(target, componentCheckType);

            if (injectionType == InjectionType.Property)
                _injectionIntoGameObjectProperty.Inject(target, componentCheckType);
        }
    }
}
