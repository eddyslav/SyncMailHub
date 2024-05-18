namespace App.Extensions;

internal static class BackgroundJobsServiceCollectionExtensions
{
	public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
	{
		return services.AddQuartz()
			.AddQuartzHostedService(options => options.WaitForJobsToComplete = true)
			.ConfigureOptions<QuartzOptionsConfigureOptions>();
	}
}
