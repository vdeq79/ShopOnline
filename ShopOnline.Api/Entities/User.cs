using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Api.Entities
{
    public class User
    {
        public enum UserRole : ushort
        {
            User = 1,
            Admin = 2
        }

        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string PasswordSalt { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; }
    }
}
