using System.Reflection;

namespace Modules.Hub.Application;

public static class AssemblyMarker
{
	public static Assembly Assembly { get; } = typeof(AssemblyMarker).Assembly;
}
