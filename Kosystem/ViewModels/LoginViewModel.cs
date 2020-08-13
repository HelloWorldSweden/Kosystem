using System.ComponentModel.DataAnnotations;

namespace Kosystem.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string? Password { get; set; }
    }
}
