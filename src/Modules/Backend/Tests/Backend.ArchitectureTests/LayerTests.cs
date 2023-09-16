using Backend.Api;
using Backend.Application;
using Backend.Domain;
using Backend.Infrastructure;
using FluentAssertions;
using NetArchTest.Rules;

namespace Backend.ArchitectureTests
{
    public class LayerTests
    {

        [Fact]
        public void PresentationLayer_Should_NotHaveDependencyOn_DomainLayer()
        {
            var domainAssemblyName = BackendDomainAssembly.Instance.GetName().Name;

            var result = Types.InAssembly(BackendPresentationAssembly.Instance)
                .Should()
                .NotHaveDependencyOn(domainAssemblyName)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void DomainLayer_Should_NotHaveDependencyOn_ApplicationLayer()
        {
            var result = Types.InAssembly(BackendDomainAssembly.Instance)
                .Should()
                .NotHaveDependencyOn(BackendApplicationAssembly.Instance.GetName().Name)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void DomainLayer_Should_NotHaveDependencyOn_InfrastructureLayer()
        {
            var result = Types.InAssembly(BackendDomainAssembly.Instance)
                .Should()
                .NotHaveDependencyOn(BackendInfrastructureAssembly.Instance.GetName().Name)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void ApplicationLayer_Should_NotHaveDependencyOn_InfrastructureLayer()
        {
            var result = Types.InAssembly(BackendApplicationAssembly.Instance)
                .Should()
                .NotHaveDependencyOn(BackendInfrastructureAssembly.Instance.GetName().Name)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
