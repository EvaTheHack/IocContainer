using IocContainer.Core.Container;
using IocContainer.Tests.Services;
using NUnit.Framework;
using System;
using System.Linq;

namespace IocContainer.Tests
{
    [TestFixture]
    public class DependencyContainerTest
    {
        private DependencyContainer container;

        [SetUp]
        public void SetUpTests()
        {
            container = new DependencyContainer();
        }

        [TearDown]
        public void Cleanup()
        {
            container = new DependencyContainer();
        }

        [Test]
        public void GetTwoSimilarServices_ResolvesThem_ObjectsAreEqual()
        {
            container.RegisterSingleton<IGuidProvider, GuidProvider>();
            container.RegisterSingleton<ILogger, Logger>();

            var providerFirst = container.GetService<IGuidProvider>();
            var providerSecond = container.GetService<IGuidProvider>();

            Assert.AreEqual(providerFirst, providerSecond);
        }

        [Test]
        public void GetService_ResolveDependency_Resolves()
        {
            container.RegisterSingleton<ILogger, Logger>();

            var logger = container.GetService<ILogger>();

            Assert.AreEqual(logger.GetType(), typeof(Logger));
        }

        [Test]
        public void GetService_ResolveExistingInternalDependency_Resolves()
        {
            container.RegisterSingleton<IGuidProvider, GuidProvider>();
            container.RegisterSingleton<ILogger, Logger>();

            var provider = container.GetService<IGuidProvider>();
            
            var logger = provider
                .GetType()
                .GetFields()
                .First(x => x.FieldType == typeof(ILogger))
                .GetValue(provider);

            Assert.AreEqual(logger.GetType(), typeof(Logger));
        }

        [Test]
        public void GetService_ResolveNotRegistredType_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => container.GetService<UknownType>());
        }

        [Test]
        public void GetService_ResolveAbstractionTypeWithoutImplementation_ThrowException()
        {
            container.RegisterSingleton<ILogger>();

            Assert.Throws<Exception>(() => container.GetService<ILogger>());
        }

        [Test]
        public void RegisterTypeWithoutPublicConstructor_ResolveIt_ThrowException()
        {
            container.RegisterSingleton<TypeWithoutPublicConstructor>();

            Assert.Throws<Exception>(() => container.GetService<TypeWithoutPublicConstructor>());
        }
    }
}
