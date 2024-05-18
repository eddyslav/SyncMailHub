using System.Reflection;

namespace Modules.Sync.Application;

public static class AssemblyMarker
{
	public static Assembly Assembly { get; } = typeof(AssemblyMarker).Assembly;
}
