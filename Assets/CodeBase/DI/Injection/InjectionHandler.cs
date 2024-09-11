namespace CodeBase.DI
{
    public class InjectionHandler
    {
        private readonly InjectionIntoField _injectionField;
        private readonly InjectionIntoProperty _injectionProperty;
        private readonly InjectionIntoMethod _injectionMethod;

        public InjectionHandler(IDependencyResolver resolver)
        {
            _injectionField = new InjectionIntoField(resolver);
            _injectionProperty = new InjectionIntoProperty(resolver);
            _injectionMethod = new InjectionIntoMethod(resolver);
        }

        public void Inject(object target, InjectionType injectionType = InjectionType.All)
        {
            if (injectionType.HasFlag(InjectionType.Field))
                _injectionField.Inject(target);

            if (injectionType.HasFlag(InjectionType.Property))
                _injectionProperty.Inject(target);

            if (injectionType.HasFlag(InjectionType.Method))
                _injectionMethod.Inject(target);
        }
    }
}
