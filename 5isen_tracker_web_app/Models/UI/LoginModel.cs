using System.ComponentModel.DataAnnotations;

namespace _5isen_tracker_web_app.Models.Ui;

public class LoginModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = "";
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }

}
