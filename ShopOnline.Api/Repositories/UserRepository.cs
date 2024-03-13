using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;
        private readonly string pepper;
        private readonly int iteration;


        public UserRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfiguration configuration = configurationBuilder.AddUserSecrets<Program>().Build();

            HashingSettings settings = configuration.GetSection("HashingSettings").Get<HashingSettings>();
            this.pepper = settings.pepper;
            this.iteration = settings.iteration;
        }


        public async Task<User> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var user = await shopOnlineDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == loginDto.Email, cancellationToken);

            if (user == null)
                throw new Exception("Email or password did not match.");

            var passwordHash = PasswordHasher.ComputeHash(loginDto.Password, user.PasswordSalt, pepper, iteration);
            if (user.PasswordHash != passwordHash)
                throw new Exception("Email or password did not match.");

            return user;
        }

        public async Task<User> Register(RegisterDto registerDto, CancellationToken cancellationToken)
        {

            var user = await shopOnlineDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == registerDto.Email, cancellationToken);

            if (user == null)
            {
                var newUser = new User
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    PasswordSalt = PasswordHasher.GenerateSalt()
                };

                newUser.PasswordHash = PasswordHasher.ComputeHash(registerDto.Password, newUser.PasswordSalt, pepper, iteration);
                await shopOnlineDbContext.Users.AddAsync(newUser, cancellationToken);
                await shopOnlineDbContext.SaveChangesAsync(cancellationToken);

                return newUser;
            }

            return null;
        }
    }


    public class HashingSettings
    {
        public string pepper { get; set; }
        public int iteration { get; set; }
    }
}
