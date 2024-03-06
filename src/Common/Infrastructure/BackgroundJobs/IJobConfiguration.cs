namespace Infrastructure.BackgroundJobs;

public interface IJobConfiguration
{
	string Name { get; }

	Type Type { get; }
}
