using System;
using System.Reflection;

namespace CodeBase.DI
{
    public class InjectionIntoProperty : IInjection
    {
        private readonly IDependencyResolver _resolver;

        public InjectionIntoProperty(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public void Inject(object target)
        {
            if (target == null)
                throw new InvalidOperationException("To inject through the property, you must specify a reference to the object!");

            PropertyInfo[] properties = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach(PropertyInfo property in properties)
            {
                if (Attribute.IsDefined(property, typeof(InjectAttribute)) && property.CanWrite)
                {
                    object value = _resolver.Resolve(property.PropertyType);
                    property.SetValue(target, value);
                }
            }
        }
    }
}
