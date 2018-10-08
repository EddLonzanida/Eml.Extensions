using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Eml.Extensions.Tests.Integration.NetCore.BaseClasses
{
    [Collection(TypeExtensionTestFixture.COLLECTION_DEFINITION)]
    public abstract class TypeExtensionTestBase
    {
        protected readonly List<Assembly> assemblies;

        protected TypeExtensionTestBase()
        {
            assemblies = TypeExtensionTestFixture.Assemblies;
        }
    }
}
