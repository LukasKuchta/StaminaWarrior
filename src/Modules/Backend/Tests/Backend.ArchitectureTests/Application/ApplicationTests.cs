using Backend.Application;
using Backend.Application.Abstractions.Commands;
using Backend.Application.Abstractions.Queries;
using Backend.Application.Contracts;
using FluentValidation;
using MediatR;
using NetArchTest.Rules;

namespace Backend.ArchitectureTests.Application
{
    public class ApplicationTests : TestBase
    {      

        [Fact]
        public void CommandHandler_Should_Have_Name_EndingWith_CommandHandler()
        {
            var result = Types.InAssembly(BackendApplicationAssembly.Instance)
                .That()
                .ImplementInterface(typeof(ICommandHandler<>))
                    .Or()
                .ImplementInterface(typeof(ICommandHandler<,>))
                .And()
                .DoNotHaveNameMatching(".*Decorator.*")
                .Should()
                .HaveNameEndingWith("CommandHandler", StringComparison.InvariantCulture)
                .GetResult();

            AssertArchTestResult(result);
        }

        [Fact]
        public void QueryHandler_Should_Have_Name_EndingWith_QueryHandler()
        {
            var result = Types.InAssembly(BackendApplicationAssembly.Instance)
                .That()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .Should()
                .HaveNameEndingWith("QueryHandler", StringComparison.InvariantCulture)
                .GetResult();

            AssertArchTestResult(result);
        }

        [Fact]
        public void Command_And_Query_Handlers_Should_Not_Be_Public_And_Sealed()
        {
            var types = Types.InAssembly(BackendApplicationAssembly.Instance)
                .That()
                    .ImplementInterface(typeof(IQueryHandler<,>))
                        .Or()
                    .ImplementInterface(typeof(ICommandHandler<>))
                        .Or()
                    .ImplementInterface(typeof(ICommandHandler<,>))
                .Should().NotBePublic().And().BeSealed()
                .GetResult().FailingTypes;

            AssertFailingTypes(types);
        }

        [Fact]
        public void Validator_Should_Have_Name_EndingWith_Validator()
        {
            var result = Types.InAssembly(BackendApplicationAssembly.Instance)
                .That()
                .Inherit(typeof(AbstractValidator<>))
                .Should()
                .HaveNameEndingWith("Validator", StringComparison.InvariantCulture)
                .GetResult();

            AssertArchTestResult(result);
        }

        [Fact]
        public void Validators_Should_Not_Be_Public_And_Sealed()
        {
            var types = Types.InAssembly(BackendApplicationAssembly.Instance)
                .That()
                .Inherit(typeof(AbstractValidator<>))
                .Should().NotBePublic().And().BeSealed()
                .GetResult().FailingTypes;

            AssertFailingTypes(types);
        }

        [Fact]
        public void Command_With_Result_Should_Not_Return_Unit()
        {
            Type commandWithResultHandlerType = typeof(ICommandHandler<,>);
            IEnumerable<Type> types = Types.InAssembly(BackendApplicationAssembly.Instance)
                .That().ImplementInterface(commandWithResultHandlerType)
                .GetTypes().ToList();

            var failingTypes = new List<Type>();
            foreach (Type type in types)
            {
                Type? interfaceType = type.GetInterface(commandWithResultHandlerType.Name);
                if (interfaceType?.GenericTypeArguments[1] == typeof(Unit))
                {
                    failingTypes.Add(type);
                }
            }

            AssertFailingTypes(failingTypes);
        }

        [Fact]
        public void MediatR_RequestHandler_Should_NotBe_Used_Directly()
        {
            var types = Types.InAssembly(BackendApplicationAssembly.Instance)
                .That()
                .ImplementInterface(typeof(IRequestHandler<>))
                .Or()
                .ImplementInterface(typeof(IRequestHandler<,>))
                .GetTypes();

            List<Type> failingTypes = new List<Type>();
            foreach (var type in types)
            {
                // direct usage is allowed only here
                if (type == typeof(IQueryHandler<,>)
                    || type == typeof(ICommandHandler<>)
                    || type == typeof(ICommandHandler<,>))
                {
                    continue;
                }

                bool isCommandHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

                bool isCommandWithResultHandler = type.GetInterfaces().Any(x =>
                    x.IsGenericType &&
                    x.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

                bool isQueryHandler = type.GetInterfaces().Any(x =>
                    x.IsGenericType &&
                    x.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

                if (!isCommandHandler && !isCommandWithResultHandler && !isQueryHandler)
                {
                    failingTypes.Add(type);
                }
            }

            AssertFailingTypes(failingTypes);
        }

        [Fact]
        public void Command_Should_Be_Immutable_And_Sealed()
        {
            var types = Types.InAssembly(BackendApplicationAssembly.Instance)
                .That()
                .Inherit(typeof(CommandBase))
                .Or()
                .Inherit(typeof(CommandBase<>))
                .Or()
                .Inherit(typeof(InternalCommandBase))
                .Or()
                .Inherit(typeof(InternalCommandBase<>))
                .Or()
                .ImplementInterface(typeof(ICommand))
                .Or()
                .ImplementInterface(typeof(ICommand<>))
                .GetTypes();

            AssertAreImmutable(types);
            AssertAreImmutable(types);
        }

        [Fact]
        public void Query_Should_Be_Immutable_And_Sealed()
        {
            var types = Types.InAssembly(BackendApplicationAssembly.Instance)
                .That()
                .ImplementInterface(typeof(IQuery<>))
                .Should().BeSealed()
                .GetTypes();

            AssertAreImmutable(types);
        }
    }
}
