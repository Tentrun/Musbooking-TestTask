using BaseServiceContracts.Consts;
using BaseServiceContracts.Interfaces.UnitOfWork;
using BaseServiceLibrary.DTO;
using BaseServiceLibrary.DTO.PartnerZoneDto;
using BaseServiceLibrary.Entity.Base;
using BaseServiceLibrary.Enum.Base;
using EventBus.RMQ.Interfaces;
using IdentityMoq;
using MediatR;

namespace BaseServiceContracts.Feature.PartnerZoneCommand.Create;

public record PartnerZoneCreateCommand : PartnerZoneCreateDto, IRequest<PartnerZoneCreateDto>
{
    internal class PartnerZoneCreateHandler : PartnerZoneHandlerBase, IRequestHandler<PartnerZoneCreateCommand, PartnerZoneCreateDto>
    {
        public PartnerZoneCreateHandler(IUnitOfWork unitOfWork, IEventBus eventBus) : base(unitOfWork, eventBus)
        {
        }

        public async Task<PartnerZoneCreateDto> Handle(PartnerZoneCreateCommand request, CancellationToken cancellationToken)
        {
            ValidateHelper.ValidateRequest(request);
            
            try
            {
                await UnitOfWork.BeginTransactionAsync(cancellationToken);
                var message = new PartnerZone
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    AccountId = request.AccountId,
                    Status = request.Status,
                    TariffId = request.TariffId,
                };
                await UnitOfWork.OutboxRegistrationAsync(message, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                await UnitOfWork.CommitTransactionAsync(cancellationToken);
                await EventBus.SendAsync(OutboxMessage.Create(message), RmqHeadEndpoints.PartnerZone, cancellationToken).ConfigureAwait(true);
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