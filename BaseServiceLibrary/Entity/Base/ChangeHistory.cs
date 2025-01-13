using System.ComponentModel.DataAnnotations.Schema;

namespace BaseServiceLibrary.Entity.Base;

public class ChangeHistory : BaseEntity
{
    public ChangeHistory()
    {
        
    }
    
    public ChangeHistory(Guid entityId, object newValue, object? prevValue = null)
    {
        EntityId = entityId;
        
        if (prevValue != null)
        {
            PrevValue = System.Text.Json.JsonSerializer.Serialize(prevValue);
        }
        
        ChangedValue = System.Text.Json.JsonSerializer.Serialize(newValue);
        ChangedDate = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Идентификатор сущности в БД
    /// </summary>
    public Guid EntityId { get; set; }
    
    /// <summary>
    /// Прошлое значение (до изменения)
    /// </summary>
    [Column(TypeName = "jsonb")]
    public string? PrevValue { get; set; }
    
    /// <summary>
    /// Измененное значение
    /// </summary>
    [Column(TypeName = "jsonb")]
    public string ChangedValue { get; set; }
    
    /// <summary>
    /// Дата изменения
    /// </summary>
    public DateTime ChangedDate { get; set; }
}