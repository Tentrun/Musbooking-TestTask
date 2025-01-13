using BaseServiceContracts.Interfaces.UnitOfWork;
using BaseServiceLibrary.Entity.Base;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace PartnerZoneService.Consumers.Consumers.Base;

public abstract class EntityBaseConsumer : IConsumer<OutboxMessage>
{
    protected readonly IUnitOfWork UnitOfWork;

    protected EntityBaseConsumer(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    protected abstract Task Handle(OutboxMessage entity, CancellationToken cancellationToken = default);
    
    public async Task Consume(ConsumeContext<OutboxMessage> context)
    {
        try
        {
            await Handle(context.Message, context.CancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}