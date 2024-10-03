using System;
using System.Reflection;
using UnityEngine;

namespace CodeBase.DI
{
    public class InjectionIntoGameObjectMethod : InjectionIntoMethod
    {
        public InjectionIntoGameObjectMethod(IDependencyResolver resolver) : base(resolver)
        {
        }

        public void Inject(MonoBehaviour target, ComponentCheckType componentCheckType = ComponentCheckType.All)
        {
            if (target == null)
                throw new InvalidOperationException("To inject through the method, you must specify a reference to the object!");

            GameObject targetGameObject = target.gameObject;
            MethodInfo[] methods = GetMethods(target.GetType());
            foreach (MethodInfo method in methods)
            {
                ParameterInfo[] parameters = method.GetParameters();
                object[] instanceParameters = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    object value = null;
                    if (componentCheckType == ComponentCheckType.ThisObject && value == null)
                        value = targetGameObject.GetComponent(parameters[i].ParameterType);

                    if (componentCheckType == ComponentCheckType.ParentObjects && value == null)
                        value = targetGameObject.GetComponentInParent(parameters[i].ParameterType);

                    if (componentCheckType == ComponentCheckType.ChildrenObjects && value == null)
                        value = targetGameObject.GetComponentInChildren(parameters[i].ParameterType);

                    if (componentCheckType == ComponentCheckType.None || value == null)
                        value = resolver.Resolve(parameters[i].ParameterType);

                    instanceParameters[i] = value;
                }

                method.Invoke(target, instanceParameters);
            }
        }
    }
}
