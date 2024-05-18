namespace Infrastructure.BackgroundJobs;

public interface IRecurringJobConfiguration : IJobConfiguration
{
	TimeSpan Schedule { get; }
}
