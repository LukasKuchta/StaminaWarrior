using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Backend.Application;
public static class BackendApplicationAssembly
{
    public static Assembly Instance => typeof(BackendApplicationAssembly).Assembly;
}

