using System;
using System.Linq;
using System.Reflection;

namespace CodeBase.DI
{
    public abstract class InstanceProviderBase : IProvider
    {
        protected IDependencyResolver resolver;
        protected BindInfo bindInfo;

        protected InstanceProviderBase(IDependencyResolver resolver, BindInfo bindInfo)
        {
            this.resolver = resolver;
            this.bindInfo = bindInfo;
        }

        public virtual object GetInstance()
        {
            return CreateInstance(bindInfo);
        }

        protected object CreateInstance(BindInfo bindInfo)
        {
            Type implementationType = bindInfo.ToTypes.First();
            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType), "Contract type cannot be null.");

            ConstructorInfo constructor = implementationType.GetConstructors().FirstOrDefault();
            if (constructor == null)
                throw new InvalidOperationException($"No public constructors found for type {implementationType.FullName}");

            ParameterInfo[] parametersInfo = constructor.GetParameters();
            if (parametersInfo.Length == 0)
                return Activator.CreateInstance(implementationType);

            object[] instacneParameters = new object[parametersInfo.Length];
            for (int i = 0; i < parametersInfo.Length; i++)
            {
                Type parameterType = parametersInfo[i].ParameterType;
                if (bindInfo.Arguments.Count > 0)
                {
                    object arg = bindInfo.Arguments.FirstOrDefault(arg => arg.Type == parameterType).Value;
                    if (arg != null)
                        instacneParameters[i] = arg;
                    else
                        throw new InvalidOperationException($"No argument found for parameter type {parameterType.FullName}");
                }
                else
                {
                    instacneParameters[i] = resolver.Resolve(parameterType);
                }
            }

            return Activator.CreateInstance(implementationType, instacneParameters);
        }
    }
}
