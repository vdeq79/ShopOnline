using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.Dtos
{
    //public sealed record UserDto(int Id, string UserName, string Email, string Role, string Token, int ExpiresIn, DateTime ExpiryTimeStamp);

    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } 
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime ExpiryTimeStamp { get; set; }

    }
    
}
