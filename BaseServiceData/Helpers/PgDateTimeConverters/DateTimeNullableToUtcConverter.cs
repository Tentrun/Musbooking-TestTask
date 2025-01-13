using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BaseServiceData.Helpers.PgDateTimeConverters;

public class DateTimeNullableToUtcConverter: ValueConverter<DateTime?, DateTime?>
{
    public DateTimeNullableToUtcConverter() : base(v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v : v.Value.ToUniversalTime()) : null, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null)
    {
    }
}