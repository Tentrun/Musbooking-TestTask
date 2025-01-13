using BaseServiceLibrary.Enum.Base;

namespace BaseServiceLibrary.DTO.PartnerZoneDto;

public record PartnerZoneCreateDto
{
    public Guid CreatorUserId { get; init; }
    
    public string Name { get; init; }  
    public string Description { get; init; }  
    public Guid AccountId { get; init; } 
    public PartnerZoneStatusEnum Status { get; init; }  
    public Guid TariffId { get; init; }  
}