using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Api.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string PasswordSalt { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

    }
}
