using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Eml.Extensions.Tests.Integration.NetCore.BaseClasses
{
    public class TypeExtensionTestFixture
    {
        public const string COLLECTION_DEFINITION = "TypeExtensionTestFixture CollectionDefinition";

        public static List<Assembly> Assemblies { get; private set; }

        public TypeExtensionTestFixture()
        {
            Assemblies = TypeExtensions.GetReferencingAssemblies(r => r.Name.StartsWith("Assembly"));
        }
    }

    [CollectionDefinition(TypeExtensionTestFixture.COLLECTION_DEFINITION)]
    public class IntegrationTestDbFixtureCollectionDefinition : ICollectionFixture<TypeExtensionTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
