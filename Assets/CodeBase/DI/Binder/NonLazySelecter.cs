using System;

namespace CodeBase.DI
{
    public class NonLazySelecter : IBindNonLazy
    {
        private readonly DIContainer _container;
        private readonly Type _contractType;

        public NonLazySelecter(DIContainer container, Type contractType)
        {
            _container = container;
            _contractType = contractType;;
        }

        public void NonLazy()
        {
            _container.RegisterNonLazy(_contractType);
        }
    }
}
