using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;
using ShopOnline.Api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await productRepository.GetItems();

                if (products == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto();
                    return Ok(productDtos);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int Id)
        {
            try
            {
                var product = await productRepository.GetItem(Id);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var productDto = product.ConvertToDto();

                    return Ok(productDto);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }

        }

        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await productRepository.GetCategories();

                var productCategoryDtos = productCategories.ConvertToDto();

                return Ok(productCategoryDtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }


        }

        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var productsByCategory = await productRepository.GetItemsByCategory(categoryId);
                var productDtos = productsByCategory.ConvertToDto();

                return Ok(productDtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }

        }
    }
}
