using BaseServiceContracts.Interfaces.UnitOfWork;
using BaseServiceLibrary.DTO.PartnerZoneDto.Get;
using EventBus.RMQ.Interfaces;
using MediatR;

namespace BaseServiceContracts.Feature.PartnerZoneCommand.Get;

public record PartnerZoneGetCommand : PartnerZoneGetDto, IRequest<PartnerZoneGetDto>
{
    internal class PartnerZoneGetHandler : PartnerZoneHandlerBase, IRequestHandler<PartnerZoneGetCommand, PartnerZoneGetDto>
    {
        public PartnerZoneGetHandler(IUnitOfWork unitOfWork, IEventBus eventBus) : base(unitOfWork, eventBus)
        {
        }

        public async Task<PartnerZoneGetDto> Handle(PartnerZoneGetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await PartnerZoneRepository.GetAllAsync(cancellationToken: cancellationToken);
                return new PartnerZoneGetDto(res.ToList());
            }
            catch (Exception ex)
            {
                //ignored
            }

            return new PartnerZoneGetDto();
        }
    }
}