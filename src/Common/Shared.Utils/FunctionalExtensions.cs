namespace Shared.Utils;

public static class FunctionalExtensions
{
	public static T TapAction<T>(this T instance, Action action)
	{
		action();

		return instance;
	}

	public static T TapAction<T>(this T instance, Action<T> action)
	{
		action(instance);

		return instance;
	}

	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
	{
		foreach (var element in enumerable)
		{
			action(element);
		}
	}
}