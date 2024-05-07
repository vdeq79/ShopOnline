using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Authentication;
using ShopOnline.Web.Services.Contracts;
using System.Security.Claims;

namespace ShopOnline.Web.Services
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IManageUserService manageUserService;
        private readonly CustomAuthenticationStateProvider customAuthenticationStateProvider;
        private const string key = "CartItemCollection";

        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService, IShoppingCartService shoppingCartService, IManageUserService manageUserService, CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
            this.localStorageService = localStorageService;
            this.shoppingCartService = shoppingCartService;
            this.manageUserService = manageUserService;
            this.customAuthenticationStateProvider = customAuthenticationStateProvider;
        }

        public async Task<List<CartItemDto>> GetCollection()
        {
            return await this.localStorageService.GetItemAsync<List<CartItemDto>>(key) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await this.localStorageService.RemoveItemAsync(key);
        }

        public async Task SaveCollection(List<CartItemDto> cartItemDtos)
        {
            await this.localStorageService.SetItemAsync(key, cartItemDtos);
        }

        private async Task<List<CartItemDto>> AddCollection()
        {
            List<CartItemDto> shoppingCartCollection = new List<CartItemDto>();

            /*if (this.manageUserService.GetCurrentUser() != null)
            {
                shoppingCartCollection = await this.shoppingCartService.GetItems(this.manageUserService.GetCurrentUser().Id);
            }*/

            var user = (await customAuthenticationStateProvider.GetAuthenticationStateAsync()).User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier);

            if(userId != null)
            {
                shoppingCartCollection = await this.shoppingCartService.GetItems(Int32.Parse(userId.Value));
            }

            if (shoppingCartCollection != null)
            {
                await this.localStorageService.SetItemAsync(key, shoppingCartCollection);
            }

            return shoppingCartCollection;
        }
    }
}
