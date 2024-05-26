using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.ComponentModel;
using System.Security.Claims;

namespace ShopOnline.Web.Pages
{
    public class CheckoutBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
        protected int TotalQty { get; set; }
        protected string PaymentDescription { get; set; }
        protected decimal PaymentAmount { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public int UserId {  get; set; } 

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
                UserId = Int32.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);


                if (ShoppingCartItems != null) 
                {
                    Guid orderGuid = Guid.NewGuid();

                    PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                    TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                    PaymentDescription = $"O_{UserId}_{orderGuid}";

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await Js.InvokeVoidAsync("initPayPalButton");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
