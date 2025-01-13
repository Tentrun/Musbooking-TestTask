using BaseServiceContracts.Interfaces.Repositories.Base;
using BaseServiceContracts.Interfaces.Repositories.Implementations;
using BaseServiceContracts.Interfaces.UnitOfWork;
using BaseServiceLibrary.Entity.Base;
using BaseServiceLibrary.Enum.Base;
using PartnerZoneService.Consumers.Consumers.Base;

namespace PartnerZoneService.Consumers.Consumers.Implementations;

public sealed class PartnerZoneConsumer : EntityBaseConsumer
{
    private readonly IPartnerZoneRepository _partnerZoneRepository;
    
    public PartnerZoneConsumer(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _partnerZoneRepository = unitOfWork.GetRepository<IPartnerZoneRepository>();
    }

    protected override async Task Handle(OutboxMessage entity, CancellationToken cancellationToken = default)
    {
        PartnerZone partnerZone = entity.GetEntity<PartnerZone>();
        await UnitOfWork.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        PartnerZone? existingEntity = await _partnerZoneRepository.GetByIdAsync(partnerZone.Id, cancellationToken).ConfigureAwait(false);
        
        try
        {
            switch (entity.OperationType)
            {
                case OutboxOperationType.CreateUpdate when existingEntity == null:
                    await _partnerZoneRepository.AddAsync(partnerZone, cancellationToken).ConfigureAwait(false);
                    await _partnerZoneRepository.SaveChangeHistoryAsync(new ChangeHistory(partnerZone.Id, partnerZone), cancellationToken);
                    break;
                case OutboxOperationType.CreateUpdate:
                    await _partnerZoneRepository.UpdateAsync(partnerZone, cancellationToken).ConfigureAwait(false);
                    await _partnerZoneRepository.SaveChangeHistoryAsync(new ChangeHistory(partnerZone.Id, partnerZone, existingEntity), cancellationToken);
                    break;
                case OutboxOperationType.ForceDelete when existingEntity != null:
                    await _partnerZoneRepository.DeleteAsync(existingEntity, cancellationToken).ConfigureAwait(false);
                    await _partnerZoneRepository.SaveChangeHistoryAsync(new ChangeHistory(partnerZone.Id, partnerZone, existingEntity), cancellationToken);
                    break;
                default:
                    throw new InvalidOperationException("Unknown operation type");
            }

            await UnitOfWork.CommitTransactionAsync(cancellationToken).ConfigureAwait(false);
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
    }
}