using System.Reflection;

using System.Text.RegularExpressions;

using Shared.Utils;

namespace App.Extensions;

internal static partial class ConfigurationBuilderExtensions
{
	private static readonly Regex nameMatcher = GenerateModuleNameMatcher();

	[GeneratedRegex(@"^Modules\.(\w+)\.", RegexOptions.Compiled)]
	private static partial Regex GenerateModuleNameMatcher();

	private static string GetModuleName(Assembly moduleAssembly)
	{
		var assemblyName = moduleAssembly.FullName!;
		var match = nameMatcher.Match(assemblyName);

		return match.Success
			? match.Groups[1].Value
			: throw new ArgumentException($"Module name cannot be extracted from \"{assemblyName}\"");
	}

	public static IConfigurationBuilder AddModulesJsonFiles(this IConfigurationBuilder builder
		, IHostEnvironment environment
		, params Assembly[] modulesAssemblies) =>
		builder.TapAction(builder => modulesAssemblies.ForEach(moduleAssembly =>
			GetModuleName(moduleAssembly)
				.TapAction(moduleName =>
					builder.AddJsonFile($"moduleSettings.{moduleName}.json", false, true)
						.AddJsonFile($"moduleSettings.{moduleName}.{environment.EnvironmentName}.json", false, true))));
}
