namespace GymManagementSystem.Application.DTOs.Package;

public class CreatePackageDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public bool IsActive { get; set; } = true;
    public int? MaxMembers { get; set; }
}

public class UpdatePackageDto : CreatePackageDto
{
    public Guid Id { get; set; }
}

public class PackageResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public bool IsActive { get; set; }
    public int? MaxMembers { get; set; }
}
