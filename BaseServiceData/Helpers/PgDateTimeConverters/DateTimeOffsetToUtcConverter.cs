using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BaseServiceData.Helpers.PgDateTimeConverters;

public class DateTimeOffsetToUtcConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
{
    public DateTimeOffsetToUtcConverter() : base(v => v.ToUniversalTime(), v => v)
    {
    }
}
