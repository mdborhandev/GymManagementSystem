using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Application.DTOs;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }
}

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Gym Name")]
    public string GymName { get; set; } = default!;

    [Display(Name = "Gym Address")]
    public string? GymAddress { get; set; }

    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = default!;

    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = default!;
}
