using BaseServiceContracts.Consts;
using MassTransit;
using PartnerZoneService.Consumers.Consumers.Implementations;

namespace PartnerZoneService.Consumers.Definitions;

public class PartnerZoneConsumerDefinition : ConsumerDefinition<PartnerZoneConsumer>
{
    public PartnerZoneConsumerDefinition()
    {
        EndpointName = RmqHeadEndpoints.PartnerZone;
        ConcurrentMessageLimit = 1;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<PartnerZoneConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
    }
}