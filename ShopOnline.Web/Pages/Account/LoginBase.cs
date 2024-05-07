using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Authorization;
using ShopOnline.Web.Authentication;

namespace ShopOnline.Web.Pages
{
    public class LoginBase : ComponentBase
    {

        [Inject]
        public IJSRuntime Js {  get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

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
                    await ((CustomAuthenticationStateProvider)AuthenticationStateProvider).UpdateAuthenticationState(userDto);
                    NavigationManager.NavigateTo("/");

                    //ManageUserService.SetCurrentUser(userDto);
                }
                else
                {
                    await Js.InvokeVoidAsync("alert", "Invalid Email or Password");
                    return;
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }
    }
}
