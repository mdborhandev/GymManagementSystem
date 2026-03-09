namespace GymManagementSystem.Domain.Common;

public abstract class BaseModel
{
    public Guid Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DateDeleted { get; set; }
}
