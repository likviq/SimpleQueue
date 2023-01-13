using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleQueue.Domain.Models
{
    public class User
    {
        [Key]
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "Length of username must be less than 25 and more than 3", MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        [StringLength(255, ErrorMessage = "Length of password must be less than 255 and more than 8", MinimumLength = 8)]
        public string Password { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        [StringLength(32, ErrorMessage = "Length of password must be less than 32 and more than 2", MinimumLength = 2)]
        public string Name { get; set; }
        [StringLength(32, ErrorMessage = "Length of password must be less than 32 and more than 2", MinimumLength = 2)]
        public string Surname { get; set; }
        public List<UserInQueue> UserInQueues { get; set; } = new List<UserInQueue>();
    }
}
