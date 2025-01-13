using System.Text.Json.Serialization;
using BaseServiceLibrary.Enum.Base;
using BaseServiceLibrary.Helpers.JSON.Serialize;

namespace BaseServiceLibrary.Entity.Base;

public sealed class OutboxMessage : BaseEntity
{
    private OutboxMessage()
    {
    }

    [JsonConstructor]
    public OutboxMessage(DateTime createdAt, string value, EntityType type, string typeName, OutboxOperationType operationType)
    {
        CreatedAt = createdAt;
        Value = value;
        Type = type;
        TypeName = typeName;
        OperationType = operationType;
    }

    public static OutboxMessage Create<T>(T entity, OutboxOperationType operationType = OutboxOperationType.CreateUpdate)
    {
        EntityType type = entity switch
        {
            PartnerZone => EntityType.PartnerZone,
            _ => throw new ArgumentException($"Unsupported type {typeof(T)}")
        };

        return new OutboxMessage
        {
            CreatedAt = DateTime.UtcNow,
            Id = Guid.NewGuid(),
            Type = type,
            TypeName = typeof(T).Name,
            Value = entity.Serialize(),
            OperationType = operationType,
        };
    }

    public DateTime CreatedAt { get; private set; }
    public string Value { get; private set; } = null!;
    public EntityType Type { get; private set; }
    public string TypeName { get; private set; } = null!;
    public OutboxOperationType OperationType { get; private set; }

    public T GetEntity<T>()
    {
        T? result = Value.Deserialize<T>();
        return result ?? throw new InvalidOperationException($"Не удалось преобразовать строку в тип {typeof(T)}");
    }
}
