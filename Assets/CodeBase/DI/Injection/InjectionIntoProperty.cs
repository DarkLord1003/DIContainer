using System;
using System.Linq;
using System.Reflection;

namespace CodeBase.DI
{
    public class InjectionIntoProperty : IInjection
    {
        protected readonly IDependencyResolver resolver;

        public InjectionIntoProperty(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Inject(object target)
        {
            if (target == null)
                throw new InvalidOperationException("To inject through the property, you must specify a reference to the object!");

            PropertyInfo[] properties = GetProperties(target.GetType());
            foreach (PropertyInfo property in properties)
            {
                object value = resolver.Resolve(property.PropertyType);
                InjectToProperty(target, property, value);
            }
        }

        protected void InjectToProperty(object target, PropertyInfo property, object value)
        {
            if (target == null)
                throw new InvalidOperationException("To inject through the property, you must specify a reference to the object!");

            if (property == null)
                throw new ArgumentNullException(nameof(property), "A reference to the property is required!");

            property.SetValue(target, value);
        }

        protected PropertyInfo[] GetProperties(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "A reference to the type is required!");

            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                       .Where(property => Attribute.IsDefined(property, typeof(InjectAttribute)) && property.CanWrite)
                       .ToArray();
        }
    }
}
