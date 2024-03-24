using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class LoginBase : ComponentBase
    {

        [Parameter]
        public string Email { get; set; }

        [Parameter]
        public string Password { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IManageUserService ManageUserService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string ErrorMessage { get; set; }

        protected async Task Login_Click()
        {
            try
            {
                var loginDto = new LoginDto(Email, Password);
                var userDto = await UserService.Login(loginDto);

                if(userDto != null)
                {
                    ManageUserService.SetCurrentUser(userDto);
                }

                NavigationManager.NavigateTo("/");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }
    }
}
