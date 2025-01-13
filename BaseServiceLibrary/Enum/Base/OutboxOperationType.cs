namespace BaseServiceLibrary.Enum.Base;

/// <summary>
/// Тип операции в Outbox
/// </summary>
public enum OutboxOperationType
{
    /// <summary>
    /// Новая запись / обновление
    /// </summary>
    CreateUpdate = 1,
    /// <summary>
    /// Полное удаление из БД
    /// </summary>
    ForceDelete
}

