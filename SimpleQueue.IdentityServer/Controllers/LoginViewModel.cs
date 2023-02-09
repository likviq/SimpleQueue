using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace SimpleQueue.IdentityServer.Controllers
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string ReturnUrl { get; set; }
        public IEnumerable<AuthenticationScheme>? ExternalProviders { get; set; }
    }
}