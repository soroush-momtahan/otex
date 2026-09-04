using System.Reflection;

namespace Otex.Micros.Identity.Application;

public static class AssemblyReference
{
    public static readonly Assembly ApplicationAssembly = typeof(AssemblyReference).Assembly;
}