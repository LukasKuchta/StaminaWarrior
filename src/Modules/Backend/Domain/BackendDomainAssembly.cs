using System.Reflection;

namespace Backend.Domain;
public static class BackendDomainAssembly
{
    public static Assembly Instance => typeof(BackendDomainAssembly).Assembly;
}

