using MyMoney.Domain.Entity.Base;

public class BaseEntity : AuditableEntity
{
	public Guid Id { get; private set; }

	public BaseEntity()
	{
		Id = Guid.NewGuid();
	}
}