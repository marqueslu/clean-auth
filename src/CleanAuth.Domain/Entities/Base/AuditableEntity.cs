namespace CleanAuth.Domain.Entities.Base;

public class AuditableEntity
{
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastUpdatedAt { get; private set; }
    public AuditableEntity() => CreatedAt = DateTime.UtcNow;
    public void SetLastUpdate() => LastUpdatedAt = DateTime.UtcNow;
}
