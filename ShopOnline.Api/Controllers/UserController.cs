using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

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

                var userDto = newUser.ConvertToDto();
                return CreatedAtAction(nameof(Register), new { id = newUser.Id}, userDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message );
            }

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login([FromBody]LoginDto loginDto, CancellationToken cancellationToken)
        {
            try
            {
                var user = await this.userRepository.Login(loginDto, cancellationToken);

                if(user == null)
                {
                    return NotFound();
                }

                var userDto = user.ConvertToDto();
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


    }
}
