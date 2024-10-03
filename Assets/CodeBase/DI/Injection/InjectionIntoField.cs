using System;
using System.Linq;
using System.Reflection;

namespace CodeBase.DI
{
    public class InjectionIntoField : IInjection
    {
        protected readonly IDependencyResolver resolver;

        public InjectionIntoField(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Inject(object target)
        {
            if (target == null)
                throw new InvalidOperationException("To inject through the field, you must specify a reference to the object!");

            FieldInfo[] fields = GetFields(target.GetType());
            foreach (FieldInfo field in fields)
            {
                object value = resolver.Resolve(field.FieldType);
                InjectToField(target, field, value);
            }
        }

        protected void InjectToField(object target, FieldInfo field, object value)
        {
            if (target == null)
                throw new InvalidOperationException("To inject through the field, you must specify a reference to the object!");

            if (field == null)
                throw new ArgumentException(nameof(field), "A reference to the field ie required!");

            field.SetValue(target, value);
        }

        protected FieldInfo[] GetFields(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "A reference to the type ie required!");

            return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                       .Where(field => Attribute.IsDefined(field, typeof(InjectAttribute)) && !field.IsInitOnly)
                       .ToArray();
        }
    }
}
