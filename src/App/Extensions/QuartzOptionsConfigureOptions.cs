using Microsoft.Extensions.Options;

using Shared.Utils;

using Infrastructure.BackgroundJobs;

namespace App.Extensions;

file static class SimpleScheduleBuilderExtensions
{
	public static SimpleScheduleBuilder ConfigureSchedule(this SimpleScheduleBuilder scheduleBuilder, IJobConfiguration jobConfiguration) =>
		jobConfiguration switch
		{
			IRecurringJobConfiguration recurringJobConfiguration => scheduleBuilder.WithInterval(recurringJobConfiguration.Schedule),
			_ => throw new InvalidOperationException($"Cannot register a trigger for {jobConfiguration.GetType()} type"),
		};
}

internal sealed class QuartzOptionsConfigureOptions(IEnumerable<IJobConfiguration> jobConfigurations) : IConfigureOptions<QuartzOptions>
{
	private readonly IReadOnlyCollection<IJobConfiguration> jobConfigurations = jobConfigurations.ToList();

	public void Configure(QuartzOptions options) =>
		jobConfigurations.ForEach(jobConfiguration => options.AddJob(jobConfiguration.Type, jobBuilder => jobBuilder.WithIdentity(jobConfiguration.Name))
			.AddTrigger(triggerBuilder => triggerBuilder.ForJob(jobConfiguration.Name)
				.WithSimpleSchedule(scheduleBuilder => scheduleBuilder
					.ConfigureSchedule(jobConfiguration)
					.RepeatForever())));
}
