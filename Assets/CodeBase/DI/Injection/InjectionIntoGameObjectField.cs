using System;
using System.Reflection;
using UnityEngine;

namespace CodeBase.DI
{
    public class InjectionIntoGameObjectField : InjectionIntoField
    {
        public InjectionIntoGameObjectField(IDependencyResolver resolver) : base(resolver)
        {
        }

        public void Inject(MonoBehaviour target, ComponentCheckType componentCheckType = ComponentCheckType.All)
        {
            if (target == null)
                throw new InvalidOperationException("To inject through the field, you must specify a reference to the object!");

            GameObject targetGameObject = target.gameObject;

            FieldInfo[] fields = GetFields(target.GetType());
            foreach (FieldInfo field in fields)
            {
                object value = null;
                if (componentCheckType == ComponentCheckType.ThisObject && value == null)
                    value = targetGameObject.GetComponent(field.FieldType);

                if (componentCheckType == ComponentCheckType.ParentObjects && value == null)
                    value = targetGameObject.GetComponentInParent(field.FieldType);

                if (componentCheckType == ComponentCheckType.ChildrenObjects && value == null)
                    value = targetGameObject.GetComponentInChildren(field.FieldType);

                if (componentCheckType == ComponentCheckType.None || value == null)
                    value = resolver.Resolve(field.FieldType);

                InjectToField(target, field, value);
            }
        }
    }
}
