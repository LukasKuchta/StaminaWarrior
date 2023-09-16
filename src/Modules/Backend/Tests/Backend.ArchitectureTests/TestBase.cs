namespace Backend.ArchitectureTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using NetArchTest.Rules;

    public abstract class TestBase
    {
        protected static void AssertAreImmutable(IEnumerable<Type> types)
        {
            IList<Type> failingTypes = new List<Type>();
            foreach (var type in types)
            {
                // check record internal setters / init set
                // https://github.com/dotnet/runtime/issues/43088
                var hasInitSetters = type.GetProperties().Where(p => p.CanWrite).Aggregate(true, (val, property) =>
                {
                    bool isExternalInit = property
                        ?.GetSetMethod()
                        ?.ReturnParameter
                        ?.GetRequiredCustomModifiers()
                        ?.Contains(typeof(System.Runtime.CompilerServices.IsExternalInit)) ?? false;

                    return isExternalInit & val;
                });


                if (type.GetFields().Any(x =>
                            !x.IsInitOnly)
                            || !hasInitSetters)
                {
                    failingTypes.Add(type);
                    break;
                }
            }

            AssertFailingTypes(failingTypes);
        }

        protected static void AssertFailingTypes(IEnumerable<Type> types)
        {
            types.Should().BeNullOrEmpty();
        }

        protected static void AssertArchTestResult(TestResult result)
        {
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
