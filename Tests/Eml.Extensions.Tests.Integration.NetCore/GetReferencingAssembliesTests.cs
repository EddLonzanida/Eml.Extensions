using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Eml.Extensions.Tests.Integration.NetCore
{
    public class GetReferencingAssembliesTests
    {
        [Fact]
        public void ShouldGetUniqueAssemblies()
        {
            var assemblyPattern = new UniqueStringPattern(new List<string> { "EmlSolutions" }).Build();
            var assemblies = TypeExtensions.GetReferencingAssemblies(assemblyPattern);

            assemblies.Count.ShouldBe(1);
        }
    }
}
