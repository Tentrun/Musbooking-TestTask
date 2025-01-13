using BaseServiceContracts.Interfaces.Repositories.Implementations;
using BaseServiceData.Contexts.PsSql;
using BaseServiceData.Repositories.Base;
using BaseServiceLibrary.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace BaseServiceData.Repositories.Implementations;

public class PartnerZoneRepository : BaseRepository<PartnerZone>, IPartnerZoneRepository
{
    public PartnerZoneRepository(PsSqlApplicationDataContext dbContext) : base(dbContext)
    {
    }

    public override Task DeleteAsync(PartnerZone entity, CancellationToken cancellationToken = default)
    {
        entity.IsArchive = true;
        entity.ArchiveAt = DateTime.UtcNow;
        
        UpdateAsync(entity, cancellationToken);
        return Task.CompletedTask;
    }
}