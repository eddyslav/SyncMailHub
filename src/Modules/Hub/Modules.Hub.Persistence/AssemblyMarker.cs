using System.Reflection;

namespace Modules.Hub.Persistence;

public static class AssemblyMarker
{
	public static Assembly Assembly { get; } = typeof(AssemblyMarker).Assembly;
}
