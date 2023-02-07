using Microsoft.AspNetCore.Authentication;

namespace SimpleQueue.IdentityServer.Controllers
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public IEnumerable<AuthenticationScheme> ExternalProviders { get; set; }
    }
}