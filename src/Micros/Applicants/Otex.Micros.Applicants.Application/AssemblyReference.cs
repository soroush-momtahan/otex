using System.Reflection;

namespace Otex.Micros.Applicants.Application;

public static class AssemblyReference
{
    public static readonly Assembly Application = typeof(AssemblyReference).Assembly;
}