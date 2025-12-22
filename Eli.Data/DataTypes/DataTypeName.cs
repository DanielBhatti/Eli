using System.Text.Json.Serialization;

namespace Eli.Data.DataTypes;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DataTypeName
{
    Unknown,
    Character,
    String,
    Boolean,
    Integer,
    Numeric,
    Date,
    Time,
    DateTime,
    DateTimeOffset,
    Guid,
}