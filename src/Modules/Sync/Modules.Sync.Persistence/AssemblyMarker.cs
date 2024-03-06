using System.Reflection;

namespace Modules.Sync.Persistence;

public static class AssemblyMarker
{
	public static Assembly Assembly { get; } = typeof(AssemblyMarker).Assembly;
}
