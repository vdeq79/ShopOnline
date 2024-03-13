using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<User> Register(RegisterDto registerDto, CancellationToken cancellationToken);

        Task<User> Login(LoginDto loginDto, CancellationToken cancellationToken);
    }
}
