using System.Net.Mime;
using App.Contracts.BLL;
using App.Public.DTO.Mappers;
using Microsoft.AspNetCore.Mvc;
using App.Public.DTO.v1;
using Asp.Versioning;
using AutoMapper;
using Base.Helpers;
using DAL.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// CartProducts Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartProductsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly CartProductMapper _mapper;

        /// <summary>
        /// CartProducts Constructor
        /// </summary>
        /// <param name="bll">Unit Of Work Interface</param>
        /// <param name="autoMapper">Auto Mapper</param>
        public CartProductsController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new CartProductMapper(autoMapper);
        }

        /// <summary>
        /// Get list of CartProducts
        /// </summary>
        /// <returns>List of all CartProducts</returns>
        // GET: api/CartProducts
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.CartProduct>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.CartProduct>>> GetCartProducts()
        {
            if (User.IsInRole("admin"))
            {
                var vm = await _bll.CartProductService.AllAsync();

                var res = vm.Select(e => _mapper.Map(e))
                    .ToList();
                return Ok(res);
            }
            else
            {
                var vm = await _bll.CartProductService.AllAsync(User.GetUserId());
                var res = vm.Select(e => _mapper.Map(e))
                    .ToList();
                return Ok(res);
            }
        }


        /// <summary>
        /// Get CartProduct by CartProduct ID.
        /// </summary>
        /// <param name="id">CartProduct ID</param>
        /// <returns>CartProduct object</returns>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.CartProduct>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        // GET: api/CartProducts/5
        public async Task<ActionResult<App.Public.DTO.v1.CartProduct>> GetCartProduct(Guid id)
        {
            var cartProduct = await _bll.CartProductService.FindAsync(id);

            if (cartProduct == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(cartProduct);

            return Ok(res);
        }


        /// <summary>
        /// Update CartProduct with specified id
        /// </summary>
        /// <param name="id">CartProduct ID</param>
        /// <param name="cartProduct">CartProduct object that need to be updated</param>
        /// <returns>Action result</returns>
        // PUT: api/CartProducts/5
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.CartProduct>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutCartProduct(Guid id, App.Public.DTO.v1.CartProduct cartProduct)
        {
            if (id != cartProduct.Id)
            {
                return BadRequest();
            }

            var bllCartProduct = _mapper.Map(cartProduct);
            _bll.CartProductService.Update(bllCartProduct!);

            await _bll.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Create new CartProduct
        /// </summary>
        /// <param name="cartProduct">New CartProduct object</param>
        /// <returns>Created CartProduct object</returns>
        // POST: api/CartProducts
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.CartProduct>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<App.Public.DTO.v1.CartProduct>> PostCartProduct(
            App.Public.DTO.v1.CartProduct cartProduct)
        {
            cartProduct.Id = Guid.NewGuid();
            var bllCartProduct = _mapper.Map(cartProduct);
            _bll.CartProductService.Add(bllCartProduct!);

            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetCartProduct", new { id = cartProduct.Id }, bllCartProduct);
        }


        /// <summary>
        /// Delete CartProduct with specified id
        /// </summary>
        /// <param name="id">CartProduct ID</param>
        /// <returns>Action result</returns>
        // DELETE: api/CartProducts/5
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.CartProduct>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCartProduct(Guid id)
        {
            var cartProduct = await _bll.CartProductService.FindAsync(id);
            if (cartProduct == null)
            {
                return NotFound();
            }

            await _bll.CartProductService.RemoveAsync(cartProduct.Id);

            // When cartProduct is deleted, then cart totalPrice and totalpriceIncludingVat should be updated
            var cart = await _bll.CartService.FirstOrDefaultAsync(cartProduct.CartId);
            if (cart != null)
            {
                cart.TotalPrice -= cartProduct.Total;
                cart.TotalPriceIncludingVat = cart.TotalPrice * (decimal)1.2;
                cart.TotalPriceIncludingVat -= cartProduct.Total * (decimal)1.2;
            }

            await _bll.SaveChangesAsync();

            return NoContent();
        }
        

        private bool CartProductExists(Guid id)
        {
            return (_bll.CartProductService.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}