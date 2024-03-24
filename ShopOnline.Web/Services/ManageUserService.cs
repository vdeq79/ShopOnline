using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services
{
    public class ManageUserService : IManageUserService
    {
        private UserDto CurrentUser;

        UserDto IManageUserService.GetCurrentUser()
        {
            return CurrentUser;
        }

        void IManageUserService.SetCurrentUser(UserDto user)
        {
            CurrentUser = user;
        }
    }
}
