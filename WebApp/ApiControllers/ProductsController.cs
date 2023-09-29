using System.Net.Mime;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Public.DTO.Mappers;
using App.Public.DTO.v1;
using Asp.Versioning;
using AutoMapper;
using DAL.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Product = App.Domain.Product;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Products Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAppBLL _bll;
        private readonly ProductMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        /// <summary>
        /// Products Constructor
        /// </summary>
        /// <param name="context">ApplicationDbContext</param>
        /// <param name="bll">Unit Of Work Interface</param>
        /// <param name="autoMapper">Auto Mapper</param>
        /// <param name="logger">Logger</param>
        public ProductsController(ApplicationDbContext context, IAppBLL bll, IMapper autoMapper,
            ILogger<ProductsController> logger)
        {
            _context = context;
            _bll = bll;
            _logger = logger;
            _mapper = new ProductMapper(autoMapper);
        }


        /// <summary>
        /// Get list of Products
        /// </summary>
        /// <returns>List of all products</returns>
        // GET: api/Products
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Product>>> GetProducts()
        {
            var vm = await _bll.ProductService.AllAsync();

            var res = vm.Select(e => _mapper.Map(e))
                .ToList();

            SetProductsActivePriceAndActiveDiscount(res);

            return Ok(res);
        }

        /// <summary>
        /// Get Product by Product ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product object</returns>
        // GET: api/Products/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<App.Public.DTO.v1.Product>> GetProduct(Guid id)
        {
            var product = await _bll.ProductService.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(product);

            SetSingleProductActivePriceAndActiveDiscount(res);
           
            return Ok(res);
        }


        /// <summary>
        /// Update Product with specified id
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="product">Product object that need to be updated</param>
        /// <returns>Action result</returns>
        // PUT: api/Products/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutProduct(Guid id, App.Public.DTO.v1.Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var bllProduct = _mapper.Map(product);
            _bll.ProductService.Update(bllProduct!);

            await _bll.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Create new Product
        /// </summary>
        /// <param name="product">New Product object</param>
        /// <returns>Created Product object</returns>
        // POST: api/Products
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Product>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> PostProduct(App.Public.DTO.v1.Product product)
        {
            product.Id = Guid.NewGuid();
            var unit = await _bll.UnitService.FirstOrDefaultAsync(product.UnitId);
            if (unit == null)
            {
                _logger.LogWarning($"Unit not found, id - {product.UnitId}");
                return NotFound();
            }

            var productType = await _bll.ProductTypeService.FirstOrDefaultAsync(product.ProductTypeId);
            if (productType == null)
            {
                _logger.LogWarning($"ProductType not found, id - {product.ProductTypeId}");
                return NotFound();
            }

            var bllProduct = _mapper.Map(product);
            _bll.ProductService.Add(bllProduct!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }


        /// <summary>
        /// Delete Product with specified id
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Action result</returns>
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _bll.ProductService.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            if (product.Discounts != null)
            {
                foreach (var productDiscount in product.Discounts)
                {
                    await _bll.DiscountService.RemoveAsync(productDiscount.Id);
                }
            }

            await _bll.ProductService.RemoveAsync(product.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(Guid id)
        {
            return (_bll.ProductService.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }


#pragma warning disable
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("single-product")]
        public async void SetSingleProductActivePriceAndActiveDiscount(App.Public.DTO.v1.Product? res)
        {
            if (res != null)
            {
                // Add active discount to product.
                var discount = _bll.DiscountService.GetActiveDiscountAsync(res.Id).Result;
                if (discount != null)
                {
                    res!.ActiveDiscount = discount.DiscountValue;
                }
                else
                {
                    res!.ActiveDiscount = null;
                }


                // Add active price to product.
                var price = _bll.PriceService.GetActivePriceAsync(res.Id).Result;
                if (price != null)
                {
                    if (res.ActiveDiscount != null)
                    {
                        res.PriceBeforeDiscounting = price.Value;
                        res.ActivePrice = Math.Round((decimal)(price.Value * (res.ActiveDiscount / 100)), 2);
                    }
                    else
                    {
                        res!.ActivePrice = price.Value;
                    }
                }
                else
                {
                    res!.ActivePrice = null;
                }

            }
        }


#pragma warning disable
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("products")]
        public async void SetProductsActivePriceAndActiveDiscount(List<App.Public.DTO.v1.Product?> res)
        {
            foreach (var re in res)
            {
                var discount = _bll.DiscountService.GetActiveDiscountAsync(re!.Id).Result;
                if (discount != null)
                {
                    re!.ActiveDiscount = discount.DiscountValue;
                }
                else
                {
                    re!.ActiveDiscount = null;
                }

                var price = _bll.PriceService.GetActivePriceAsync(re.Id).Result;
                if (price != null)
                {
                    if (re.ActiveDiscount != null)
                    {
                        re.PriceBeforeDiscounting = price.Value;
                        re.ActivePrice = Math.Round((decimal)(price.Value * (1 - ((decimal)re.ActiveDiscount / 100))),
                            2);
                    }
                    else
                    {
                        re!.ActivePrice = price.Value;
                    }
                }
                else
                {
                    re!.ActivePrice = null;
                }
            }
        }
    }
}