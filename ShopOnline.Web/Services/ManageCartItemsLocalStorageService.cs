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
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private const string CartItemCollection = "CartItemCollection";
        private const string CartId = "CartId";


        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService, IShoppingCartService shoppingCartService, IManageUserService manageUserService, AuthenticationStateProvider authenticationStateProvider)
        {
            this.localStorageService = localStorageService;
            this.shoppingCartService = shoppingCartService;
            this.manageUserService = manageUserService;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<int> GetCartId()
        {
            return await this.localStorageService.GetItemAsync<int?>(CartId) ?? await AddCartId();
        }

        public async Task<List<CartItemDto>> GetCollection()
        {
            return await this.localStorageService.GetItemAsync<List<CartItemDto>>(CartItemCollection) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await this.localStorageService.RemoveItemAsync(CartItemCollection);
        }

        public async Task SaveCollection(List<CartItemDto> cartItemDtos)
        {
            await this.localStorageService.SetItemAsync(CartItemCollection, cartItemDtos);
        }

        private async Task<List<CartItemDto>> AddCollection()
        {
            List<CartItemDto> shoppingCartCollection = new List<CartItemDto>();

            /*if (this.manageUserService.GetCurrentUser() != null)
            {
                shoppingCartCollection = await this.shoppingCartService.GetItems(this.manageUserService.GetCurrentUser().Id);
            }*/

            var user = (await authenticationStateProvider.GetAuthenticationStateAsync()).User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier);

            if(userId != null)
            {
                shoppingCartCollection = await this.shoppingCartService.GetItems(Int32.Parse(userId.Value));
            }

            if (shoppingCartCollection != null)
            {
                await this.localStorageService.SetItemAsync(CartItemCollection, shoppingCartCollection);
            }

            return shoppingCartCollection;
        }

        private async Task<int> AddCartId()
        {
            int cartId = -1;

            var user = (await authenticationStateProvider.GetAuthenticationStateAsync()).User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                cartId = await this.shoppingCartService.GetCartId(Int32.Parse(userId.Value));
            }

            if (cartId != -1)
            {
                await this.localStorageService.SetItemAsync(CartId, cartId);
            }

            return cartId;
        }

        public async Task RemoveCartId()
        {
            await this.localStorageService.RemoveItemAsync(CartId);
        }
    }
}
