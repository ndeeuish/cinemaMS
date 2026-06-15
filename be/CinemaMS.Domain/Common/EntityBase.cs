namespace CinemaMS.Domain.Common;

public abstract class EntityBase<TId>
{
    public TId Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
}

public abstract class EntityBase : EntityBase<int>
{
}
