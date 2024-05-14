using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public ProductDto Product { get; set; }
        public string ErrorMessage { get; set; }

        public int CartId { get; set; }
        private List<CartItemDto> ShoppingCartItems { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                CartId = await ManageCartItemsLocalStorageService.GetCartId();
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                Product = await GetProductById(Id);
            }
            catch(Exception ex) 
            {
                ErrorMessage = ex.Message;
            }   
        }

        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);

                if(cartItemDto != null)
                {
                    int index = ShoppingCartItems.FindIndex(c => c.Id == cartItemDto.Id);

                    //Console.WriteLine(ShoppingCartItems.First().Qty);

                    if (index != -1)
                    {
                        ShoppingCartItems[index] = cartItemDto;
                    }
                    else
                    {
                        ShoppingCartItems.Add(cartItemDto);
                    }

                    //Console.WriteLine(ShoppingCartItems.First().Qty);

                    await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                }

                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<ProductDto> GetProductById(int id)
        {
            var productDtos = await ManageProductsLocalStorageService.GetCollection();

            if(productDtos != null)
            {
                return productDtos.SingleOrDefault(p => p.Id == id);
            }

            return null;

        }

    }
}
