namespace GymManagementSystem.Domain.Common;

public abstract class BaseModel
{
    public Guid Id { get; set; } = default!;
    
    // SaaS Multi-Tenancy
    public Guid CompanyId { get; set; }
    
    // Auditing
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Soft Delete
    public bool IsDelete { get; set; }
    public DateTime? DateDeleted { get; set; }
}
