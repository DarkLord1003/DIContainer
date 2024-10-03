using OpenCover.Framework.Model;
using System;
using System.Linq;
using System.Reflection;

namespace CodeBase.DI
{
    public class InjectionIntoMethod : IInjection
    {
        protected readonly IDependencyResolver resolver;

        public InjectionIntoMethod(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Inject(object target)
        {
            if(target == null)
               throw new InvalidOperationException("To inject through the method, you must specify a reference to the object!");

            MethodInfo[] methods = GetMethods(target.GetType());
            foreach (MethodInfo method in methods)
            {
                InjectToMethod(target, method);
            }
        }

        protected void InjectToMethod(object target, MethodInfo method)
        {
            if (target == null)
                throw new InvalidOperationException("To inject through the method, you must specify a reference to the object!");

            if (method == null)
                throw new ArgumentNullException(nameof(method), "A reference to the method is required!");

            ParameterInfo[] parameters = method.GetParameters();
            object[] instanceParameters = new object[parameters.Length];
            for (int i = 0; i < instanceParameters.Length; i++)
            {
                instanceParameters[i] = resolver.Resolve(parameters[i].ParameterType);
            }

            method.Invoke(target, instanceParameters);
        }

        protected MethodInfo[] GetMethods(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "A reference to the type is required!");


            return type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                       .Where(method => Attribute.IsDefined(method, typeof(InjectAttribute)))
                       .ToArray();
        }
    }
}
