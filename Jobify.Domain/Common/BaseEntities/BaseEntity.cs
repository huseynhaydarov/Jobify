namespace Jobify.Domain.Common.BaseEntities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
}
