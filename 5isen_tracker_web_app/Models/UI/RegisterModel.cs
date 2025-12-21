using System.ComponentModel.DataAnnotations;

namespace _5isen_tracker_web_app.Models.Ui;

public class RegisterModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, MinLength(8), DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Required, Compare(nameof(Password)), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = "";
    public string? ReturnUrl { get; set; }

}
