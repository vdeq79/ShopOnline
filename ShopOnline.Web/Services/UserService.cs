using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/User/Login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<UserDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message:{message}");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/User/Register", registerDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<UserDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message:{message}");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
