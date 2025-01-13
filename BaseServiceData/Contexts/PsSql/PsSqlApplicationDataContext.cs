using BaseServiceData.Helpers.PgDateTimeConverters;
using BaseServiceLibrary.Entity.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BaseServiceData.Contexts.PsSql;

public class PsSqlApplicationDataContext : DbContext
{
    public PsSqlApplicationDataContext(DbContextOptions<PsSqlApplicationDataContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        //Исправления некоторых проблем при конвертации в DT, которые так и не были доделаны в npgsql
        
        configurationBuilder
            .Properties<DateTime>()
            .HaveConversion<DateTimeToUtcConverter>();

        configurationBuilder
            .Properties<DateTime?>()
            .HaveConversion<DateTimeNullableToUtcConverter>();

        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetToUtcConverter>();
    }
    
    public DbSet<PartnerZone> PartnerZones { get; set; }
    
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    public DbSet<ChangeHistory> ChangeHistory { get; set; }
}