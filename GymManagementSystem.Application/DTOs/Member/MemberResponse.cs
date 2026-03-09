namespace GymManagementSystem.Application.DTOs.Member;

public class MemberResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime JoinedDate { get; set; }
    public Guid GymId { get; set; }
}
