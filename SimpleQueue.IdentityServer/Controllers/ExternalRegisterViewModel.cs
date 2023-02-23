namespace SimpleQueue.IdentityServer.Controllers
{
    public class ExternalRegisterViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string ReturnUrl { get; set; }
    }
}
