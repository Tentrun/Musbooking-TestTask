using BaseServiceLibrary.Enum.Base;

namespace BaseServiceLibrary.Entity.Base;

public class PartnerZone : BaseEntity
{ 
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AccountId { get; set; } = default;
    public PartnerZoneStatusEnum Status { get; set; } = PartnerZoneStatusEnum.Unknown;  
    public Guid TariffId { get; set; } = default;
    public DateTime? ArchiveAt { get; set; } = null;
    public bool IsArchive { get; set; } = false; //Признак архивации, вместо удаления
}
