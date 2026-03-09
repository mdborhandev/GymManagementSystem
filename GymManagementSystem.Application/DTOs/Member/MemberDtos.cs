using GymManagementSystem.Domain.Enums;

namespace GymManagementSystem.Application.DTOs.Member;

public class CreateMemberDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Email { get; set; }
    public string Phone { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Address { get; set; }
    public Guid PackageId { get; set; }
    public MemberStatus Status { get; set; } = MemberStatus.Active;
}

public class UpdateMemberDto : CreateMemberDto
{
    public Guid Id { get; set; }
}

// Ensure MemberResponse is aligned
public class MemberResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime JoinedDate { get; set; }
    public string Status { get; set; } = default!;
    public int StatusEnumValue { get; set; }
    public string PackageName { get; set; } = default!;
    public Guid PackageId { get; set; }
    public Guid CompanyId { get; set; }
}
