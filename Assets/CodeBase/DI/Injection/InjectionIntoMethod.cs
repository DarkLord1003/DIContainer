using System;
using System.Reflection;

namespace CodeBase.DI
{
    public class InjectionIntoMethod : IInjection
    {
        private readonly IDependencyResolver _resolver;

        public InjectionIntoMethod(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public void Inject(object target)
        {
            if(target == null)
               throw new InvalidOperationException("To inject through the method, you must specify a reference to the object!");

            MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (MethodInfo method in methods)
            {
                if (Attribute.IsDefined(method, typeof(InjectAttribute)))
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    object[] instanceParameters = new object[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        instanceParameters[i] = _resolver.Resolve(parameters[i].ParameterType);
                    }

                    method.Invoke(target, instanceParameters);
                }
            }
        }
    }
}
