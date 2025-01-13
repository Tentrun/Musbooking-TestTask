using BaseServiceLibrary.Entity.Base;
using BaseServiceLibrary.Enum.Base;

namespace BaseServiceLibrary.DTO.PartnerZoneDto.Get;

public record PartnerZoneGetDto
{
    public PartnerZoneGetDto()
    {
        
    }
    
    public PartnerZoneGetDto(List<PartnerZone> partnerZones)
    {
        PartnerZones = new List<PartnerZoneDto>();
        foreach (var partnerZone in partnerZones)
        {
            PartnerZones.Add(new PartnerZoneDto
            {
                Id = partnerZone.Id,
                Name = partnerZone.Name,
                Description = partnerZone.Description,
                AccountId = partnerZone.AccountId,
                Status = partnerZone.Status,
                TariffId = partnerZone.TariffId,
                ArchiveAt = partnerZone.ArchiveAt,
                IsArchive = partnerZone.IsArchive,
            });
        }
    }

    public List<PartnerZoneDto> PartnerZones { get; init; }
}

public class PartnerZoneDto
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AccountId { get; set; } = default;
    public PartnerZoneStatusEnum Status { get; set; } = PartnerZoneStatusEnum.Unknown;  
    public Guid TariffId { get; set; } = default;
    public DateTime? ArchiveAt { get; set; } = null;
    public bool IsArchive { get; set; } = false; //Признак архивации, вместо удаления
}