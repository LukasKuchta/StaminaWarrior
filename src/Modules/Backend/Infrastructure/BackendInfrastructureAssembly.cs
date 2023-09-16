using System.Reflection;

namespace Backend.Infrastructure;
public static class BackendInfrastructureAssembly
{
    public static Assembly Instance => typeof(BackendInfrastructureAssembly).Assembly;
}
