using System;
using System.Collections.Generic;
using System.Linq;

namespace IocContainer.Core.Container
{
    public class DependencyContainer
    {
        private readonly List<TypeDescriptor> _registeredObjects = new List<TypeDescriptor>();

        public void RegisterSingleton<TService>()
        {
            _registeredObjects.Add(new TypeDescriptor(typeof(TService)));
        }

        public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
        {
            _registeredObjects.Add(new TypeDescriptor(typeof(TService), typeof(TImplementation)));
        }

        public void RegisterSingleton<TService>(TService implementation)
        {
            _registeredObjects.Add(new TypeDescriptor(implementation));
        }

        public object GetService(Type serviceType)
        {
            var descriptor = _registeredObjects.FirstOrDefault(x => x.ServiceType == serviceType);
            if (descriptor == null)
            {
                throw new ArgumentException($"Service of type {serviceType.Name} isn't registered");
            }

            if (descriptor.Implementation != null)
            {
                return descriptor.Implementation;
            }

            var actualType = descriptor.ImplementationType ?? descriptor.ServiceType;

            if (actualType.IsAbstract || actualType.IsInterface)
            {
                throw new Exception("Cannot instantiate abstract classes or interfaces");
            }

            var constructorInfo = actualType.GetConstructors().FirstOrDefault();
            if(constructorInfo == null)
            {
                throw new Exception("Cannot instantiate class without public constructor");
            }

            var parameters = constructorInfo
                .GetParameters()
                .Select(x => GetService(x.ParameterType))
                .ToArray();

            descriptor.Implementation = Activator.CreateInstance(actualType, parameters);
            
            return descriptor.Implementation;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }
    }
}
