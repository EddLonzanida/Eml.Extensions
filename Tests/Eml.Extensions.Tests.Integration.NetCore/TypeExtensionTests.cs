using Eml.Extensions.Tests.Integration.NetCore.BaseClasses;
using Shouldly;
using System.Composition;
using System.Linq;
using Xunit;

namespace Eml.Extensions.Tests.Integration.NetCore
{
    public class TypeExtensionTests : TypeExtensionTestBase
    {
        [Fact]
        public void ShouldGetAssemblies()
        {
            assemblies.Count.ShouldBe(2);
        }

        [Fact]
        public void ShouldGetClassNames()
        {
            var sut = assemblies.SelectMany(r => r.GetClasses());

            sut.Count().ShouldBe(5);
        }

        [Fact]
        public void ShouldGetInterfaceNames()
        {
            var sut = assemblies.SelectMany(r => r.GetInterfaceNames());

            sut.Count().ShouldBe(2);
        }

        [Fact]
        public void ShouldGetMethodNames()
        {
            var classes = assemblies.SelectMany(r => r.GetClasses());

            var sut = classes.SelectMany(r => r.GetMethodNames());

            sut.Count().ShouldBe(28);
        }

        [Fact]
        public void ShouldGetAllPropertyNames()
        {
            var classes = assemblies.SelectMany(r => r.GetClasses());

            var sut = classes.SelectMany(r => r.GetPropertyNames());

            sut.Count().ShouldBe(18);
        }

        [Fact]
        public void ShouldGetClassAttributes()
        {
            var classes = assemblies.SelectMany(r => r.GetClasses());

            var sut = classes.Select(r => r.GetClassAttribute<ExportAttribute>())
                .Where(r => r != null);

            sut.Count().ShouldBe(4);
        }

        [Fact]
        public void ShouldGetPropertyAttributes()
        {
            var classes = assemblies.SelectMany(r => r.GetClasses());

            var sut = classes.SelectMany(r => r.GetProperties2())
                .Select(r => r.GetPropertyAttribute<ImportAttribute>())
                .Where(r => r != null);

            sut.Count().ShouldBe(8);
        }
    }
}
