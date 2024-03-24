using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IManageUserService
    {
        UserDto GetCurrentUser();

        void SetCurrentUser(UserDto user);
    }
}
