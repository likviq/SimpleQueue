using System.ComponentModel.DataAnnotations;

namespace SimpleQueue.IdentityServer.Controllers
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ReturnUrl { get; set; }
    }
}