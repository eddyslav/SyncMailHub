using System.Reflection;

namespace Modules.Notifications.Persistence;

public static class AssemblyMarker
{
	public static Assembly Assembly { get; } = typeof(AssemblyMarker).Assembly;
}
