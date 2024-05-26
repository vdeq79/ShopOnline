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
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private const string CartItemCollection = "CartItemCollection";
        private const string CartId = "CartId";

        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService, IShoppingCartService shoppingCartService, AuthenticationStateProvider authenticationStateProvider)
        {
            this.localStorageService = localStorageService;
            this.shoppingCartService = shoppingCartService;
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
            int? userId = await GetUserId();

            if(userId != null)
            {
                var token = await ((CustomAuthenticationStateProvider)authenticationStateProvider).GetToken();
                shoppingCartCollection = await this.shoppingCartService.GetItems(userId.Value, token);
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
            int? userId = await GetUserId();

            if (userId != null)
            {
                var token = await ((CustomAuthenticationStateProvider)authenticationStateProvider).GetToken();
                cartId = await this.shoppingCartService.GetCartId(userId.Value, token);
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

        private async Task<int?> GetUserId()
        {

            var user = (await authenticationStateProvider.GetAuthenticationStateAsync()).User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                return Int32.Parse(userId.Value);
            }

            return null;
        }
    }
}
