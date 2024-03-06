using Modules.Hub.Infrastucture.Communication;
using Modules.Hub.Infrastucture.Communication.RegisterServiceAccountStateChange;

namespace Modules.Hub.Infrastucture;

public static class BusRegistrationConfiguratorExtensions
{
	public static IBusRegistrationConfigurator AddHubConsumers(this IBusRegistrationConfigurator configurator) =>
		configurator.TapAction(configurator => configurator.AddConsumer<GetAccountCredentialsConsumer>())
			.TapAction(configurator => configurator.AddConsumer<RegisterServiceAccountStateChangeConsumer>());
}
