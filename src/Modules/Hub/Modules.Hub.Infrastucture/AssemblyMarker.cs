using System.Reflection;

namespace Modules.Hub.Infrastucture;

public static class AssemblyMarker
{
	public static Assembly Assembly { get; } = typeof(AssemblyMarker).Assembly;
}
