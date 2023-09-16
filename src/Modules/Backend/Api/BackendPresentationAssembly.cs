using System.Reflection;

namespace Backend.Api;

public static class BackendPresentationAssembly
{
    public static Assembly Instance => typeof(BackendPresentationAssembly).Assembly;
}
