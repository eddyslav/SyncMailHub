using System.Reflection;

namespace Modules.Notifications.Application;

public static class AssemblyMarker
{
	public static Assembly Assembly { get; } = typeof(AssemblyMarker).Assembly;
}
