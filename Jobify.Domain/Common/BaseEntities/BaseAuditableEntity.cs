namespace Jobify.Domain.Common.BaseEntities;

public abstract class BaseAuditableEntity : BaseEntity
{
    public Guid? CreatedById { get; set; }
    public string? CreatedBy { get; set; }

    public Guid? ModifiedById { get; set; }
    public string? ModifiedBy { get; set; }
}
