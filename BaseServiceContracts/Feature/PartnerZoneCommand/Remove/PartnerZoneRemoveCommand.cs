using BaseServiceContracts.Consts;
using BaseServiceContracts.Interfaces.UnitOfWork;
using BaseServiceLibrary.DTO.PartnerZoneDto.Remove;
using BaseServiceLibrary.Entity.Base;
using BaseServiceLibrary.Enum.Base;
using EventBus.RMQ.Interfaces;
using IdentityMoq;
using MediatR;

namespace BaseServiceContracts.Feature.PartnerZoneCommand.Remove;

public record PartnerZoneRemoveCommand : PartnerZoneRemoveDto, IRequest<PartnerZoneRemoveDto>
{
    internal class PartnerZoneRemoveHandler : PartnerZoneHandlerBase, IRequestHandler<PartnerZoneRemoveCommand, PartnerZoneRemoveDto>
    {
        public PartnerZoneRemoveHandler(IUnitOfWork unitOfWork, IEventBus eventBus) : base(unitOfWork, eventBus)
        {
        }

        public async Task<PartnerZoneRemoveDto> Handle(PartnerZoneRemoveCommand request, CancellationToken cancellationToken)
        {
            await ValidateHelper.ValidateRequest(request);
            
            PartnerZone? existingPartnerZone = await PartnerZoneRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingPartnerZone == null)
            {
                throw new ArgumentException($"Partner zone with id: {request.Id} does not exist for remove operation");
            }
            
            try
            {
                await UnitOfWork.BeginTransactionAsync(cancellationToken);
                var message = new PartnerZone
                {
                    Id = request.Id,
                };
                await UnitOfWork.OutboxRegistrationAsync(message, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                await UnitOfWork.CommitTransactionAsync(cancellationToken);
                await EventBus.SendAsync(OutboxMessage.Create(message, OutboxOperationType.ForceDelete), RmqHeadEndpoints.PartnerZone, cancellationToken).ConfigureAwait(true);
            }
            catch (Exception e)
            {
                await UnitOfWork.RollbackTransactionAsync(cancellationToken).ConfigureAwait(false);
                throw;
            }
            finally
            {
                await UnitOfWork.SaveAsync(cancellationToken);
            }

            return request;
        }
    }
}