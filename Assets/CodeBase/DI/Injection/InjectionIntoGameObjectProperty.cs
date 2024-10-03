using System;
using System.Reflection;
using UnityEngine;

namespace CodeBase.DI
{
    public class InjectionIntoGameObjectProperty : InjectionIntoProperty
    {
        public InjectionIntoGameObjectProperty(IDependencyResolver resolver) : base(resolver)
        {
        }

        public void Inject(MonoBehaviour target, ComponentCheckType componentCheckType = ComponentCheckType.All)
        {
            if (target == null)
                throw new InvalidOperationException("To inject through the property, you must specify a reference to the object!");

            GameObject targetGameObject = target.gameObject;
            PropertyInfo[] properties = GetProperties(target.GetType());
            foreach (PropertyInfo property in properties)
            {
                object value = null;
                if (componentCheckType == ComponentCheckType.ThisObject && value == null)
                    value = targetGameObject.GetComponent(property.PropertyType);

                if (componentCheckType == ComponentCheckType.ParentObjects && value == null)
                    value = targetGameObject.GetComponentInParent(property.PropertyType);

                if (componentCheckType == ComponentCheckType.ChildrenObjects && value == null)
                    value = targetGameObject.GetComponentInChildren(property.PropertyType);

                if (componentCheckType == ComponentCheckType.None || value == null)
                    value = resolver.Resolve(property.PropertyType);

                property.SetValue(target, value);
            }
        }
    }
}
