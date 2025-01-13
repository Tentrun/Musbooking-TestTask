using System.Reflection;
using BaseServiceContracts.Consts;
using BaseServiceContracts.Interfaces.UnitOfWork;
using BaseServiceLibrary.DTO.PartnerZoneDto;
using BaseServiceLibrary.DTO.PartnerZoneDto.Update;
using BaseServiceLibrary.Entity.Base;
using EventBus.RMQ.Interfaces;
using IdentityMoq;
using MediatR;

namespace BaseServiceContracts.Feature.PartnerZoneCommand.Update;

public record PartnerZoneUpdateCommand : PartnerZoneUpdateDto, IRequest<PartnerZoneUpdateDto>
{
    internal class PartnerZoneUpdateHandler : PartnerZoneHandlerBase, IRequestHandler<PartnerZoneUpdateCommand, PartnerZoneUpdateDto>
    {
        public PartnerZoneUpdateHandler(IUnitOfWork unitOfWork, IEventBus eventBus) : base(unitOfWork, eventBus)
        {
        }

        public async Task<PartnerZoneUpdateDto> Handle(PartnerZoneUpdateCommand request, CancellationToken cancellationToken)
        {
            PartnerZone? existingPartnerZone = await PartnerZoneRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingPartnerZone == null)
            {
                throw new ArgumentException($"Partner zone with id: {request.Id} does not exist for update operation");
            }
            
            await ValidateHelper.ValidateRequest(request, existingPartnerZone);
         
            
            try
            {
                PartnerZone partnerZoneFromRequest = new PartnerZone();
                
                switch (IdentityUsers.IdentityUsersList.First(x => x.Id == request.CreatorUserId).Role)
                {
                    case IdentityRoles.Admin:
                        partnerZoneFromRequest = new PartnerZone
                        {
                            Id = request.Id,
                            Name = request.Name ?? existingPartnerZone.Name,
                            Description = request.Description ?? existingPartnerZone.Description,
                            AccountId = request.AccountId ?? existingPartnerZone.AccountId,
                            Status = request.Status ?? existingPartnerZone.Status,
                            TariffId = request.TariffId ?? existingPartnerZone.TariffId,
                            ArchiveAt = existingPartnerZone.ArchiveAt,
                            IsArchive = existingPartnerZone.IsArchive
                        };
                        break;
                    case IdentityRoles.Verified:
                        partnerZoneFromRequest = new PartnerZone
                        {
                            Id = request.Id,
                            Name = request.Name ?? existingPartnerZone.Name,
                            Description = request.Description ?? existingPartnerZone.Description,
                            AccountId = existingPartnerZone.AccountId,
                            Status = existingPartnerZone.Status,
                            TariffId =existingPartnerZone.TariffId,
                            ArchiveAt = existingPartnerZone.ArchiveAt,
                            IsArchive = existingPartnerZone.IsArchive
                        };
                        break;
                }
                
                await UnitOfWork.BeginTransactionAsync(cancellationToken);
                await UnitOfWork.OutboxRegistrationAsync(partnerZoneFromRequest, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                await UnitOfWork.CommitTransactionAsync(cancellationToken);
                await EventBus.SendAsync(OutboxMessage.Create(partnerZoneFromRequest), RmqHeadEndpoints.PartnerZone, cancellationToken).ConfigureAwait(true);
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