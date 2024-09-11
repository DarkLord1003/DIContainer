using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeBase.DI
{
    public class InjectionIntoField : IInjection
    {
        private readonly IDependencyResolver _resolver;

        public InjectionIntoField(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public void Inject(object target)
        {
            if (target == null)
                throw new InvalidOperationException("To inject through the field, you must specify a reference to the object!");

            FieldInfo[] fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                if (Attribute.IsDefined(field, typeof(InjectAttribute)))
                {
                    object value = _resolver.Resolve(field.FieldType);
                    field.SetValue(target, value);
                }
            }
        }
    }
}
