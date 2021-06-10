using System;

namespace IocContainer.Core.Container
{
    public class TypeDescriptor
    {
        public TypeDescriptor(object implementation)
        {
            ServiceType = implementation.GetType();
            Implementation = implementation;
        }

        public TypeDescriptor(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public TypeDescriptor(Type serviceType, Type implementationType)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
        }

        public Type ServiceType { get; set; }
        public Type ImplementationType { get; set; }
        public object Implementation { get; internal set; }
    }
}
