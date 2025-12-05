namespace Jobify.Domain.Common.BaseEntities;

public abstract class BaseAuditableEntity : BaseEntity
{
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
}
