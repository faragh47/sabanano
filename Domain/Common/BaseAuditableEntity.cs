namespace CleanArchitecture.Domain.Common;

public abstract class BaseAuditableEntity<Tkey> : BaseEntity<Tkey>
{
    public DateTime Created { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public long? LastModifiedBy { get; set; }

    public bool IsActive { get; set; }
}
