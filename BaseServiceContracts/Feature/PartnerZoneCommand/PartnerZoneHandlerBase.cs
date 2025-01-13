using BaseServiceContracts.Feature.PartnerZoneCommand.Remove;
using BaseServiceContracts.Interfaces.Repositories.Implementations;
using BaseServiceContracts.Interfaces.UnitOfWork;
using EventBus.RMQ.Interfaces;

namespace BaseServiceContracts.Feature.PartnerZoneCommand;

public class PartnerZoneHandlerBase
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IPartnerZoneRepository PartnerZoneRepository;
    protected readonly IEventBus EventBus;

    protected PartnerZoneHandlerBase(IUnitOfWork unitOfWork, IEventBus eventBus)
    {
        UnitOfWork = unitOfWork;
        EventBus = eventBus;
        PartnerZoneRepository = unitOfWork.GetRepository<IPartnerZoneRepository>();
    }
}