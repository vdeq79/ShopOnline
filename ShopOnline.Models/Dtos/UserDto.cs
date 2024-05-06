using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.Dtos
{
    public sealed record UserDto(int Id, string UserName, string Email, string Role, string Token, int ExpiresIn, DateTime ExpiryTimeStamp);
    
}
