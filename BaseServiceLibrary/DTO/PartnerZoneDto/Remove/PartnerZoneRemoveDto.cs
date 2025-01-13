namespace BaseServiceLibrary.DTO.PartnerZoneDto.Remove;

public record PartnerZoneRemoveDto
{
    public Guid CreatorUserId { get; init; }
    public Guid Id { get; init; }
}