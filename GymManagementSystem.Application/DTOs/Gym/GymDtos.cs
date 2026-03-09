namespace GymManagementSystem.Application.DTOs.Gym;

public class CreateGymDto
{
    public string Name { get; set; } = default!;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
}

public class UpdateGymDto : CreateGymDto
{
    public Guid Id { get; set; }
}

public class GymResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
}
