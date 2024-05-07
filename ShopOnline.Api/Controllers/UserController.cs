using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopOnline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        public const string JWT_SECURITY_KEY = "this is my custom Secret key for authentication";
        private const int JWT_TOKEN_VALIDITY_MINS = 20;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody]RegisterDto registerDto, CancellationToken cancellationToken)
        {
            try
            {
                var newUser = await this.userRepository.Register(registerDto, cancellationToken);
            
                if(newUser == null)
                {
                    throw new Exception($"The Email is already registered");
                }

                //var userDto = newUser.ConvertToDto();
                //return CreatedAtAction(nameof(Register), new { id = newUser.Id}, userDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message );
            }

        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Login([FromBody]LoginDto loginDto, CancellationToken cancellationToken)
        {
            try
            {
                var user = await this.userRepository.Login(loginDto, cancellationToken);

                if(user == null)
                {
                    return NoContent();
                }

                var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
                var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

                var claimsIdentity = new ClaimsIdentity( new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, nameof(user.Role)),
                    new Claim(ClaimTypes.Email, user.Email)
                });

                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature);

                var securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Expires = tokenExpiryTimeStamp,
                    SigningCredentials = signingCredentials
                };

                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
                var token = jwtSecurityTokenHandler.WriteToken(securityToken);

                var userDto = user.ConvertToDto(token, (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


    }
}
