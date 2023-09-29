using System.Net.Mime;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Public.DTO.Mappers;
using App.Public.DTO.v1;
using Asp.Versioning;
using AutoMapper;
using Base.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using CartProduct = App.BLL.DTO.CartProduct;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Carts Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly CartMapper _mapper;

        /// <summary>
        /// Carts Constructor
        /// </summary>
        /// <param name="bll">Unit Of Work Interface</param>
        /// <param name="autoMapper">Auto Mapper</param>
        public CartsController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new CartMapper(autoMapper);
        }


        /// <summary>
        /// Get list of Carts
        /// </summary>
        /// <returns>List of all Carts</returns>
        // GET: api/Carts
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Cart>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Cart>>> GetCarts()
        {
            if (User.IsInRole("admin"))
            {
                var vm = await _bll.CartService.AllAsync();

                var res = vm.Select(e => _mapper.Map(e))
                    .ToList();
                return Ok(res);
            }
            else
            {
                var vm = await _bll.CartService.AllAsync(User.GetUserId());
                var res = vm.Select(e => _mapper.Map(e))
                    .ToList();
                return Ok(res);
            }
        }


        /// <summary>
        ///  Get Cart by Cart ID
        /// </summary>
        /// <param name="id">Cart ID</param>
        /// <returns>Cart object</returns>
        // GET: api/Carts/5
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Cart>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<App.Public.DTO.v1.Cart>> GetCart(Guid id)
        {
            if (User.IsInRole("admin"))
            {
                var vm = await _bll.CartService.FindAsync(id);
                if (vm == null)
                {
                    return NotFound();
                }

                var res = _mapper.Map(vm);
                return Ok(res);
            }
            else
            {
                var vm = await _bll.CartService.FindAsync(id, User.GetUserId());
                var res = _mapper.Map(vm);
                return Ok(res);
            }
        }


        /// <summary>
        /// Update Cart with specified id
        /// </summary>
        /// <param name="id">Cart ID</param>
        /// <param name="cart">Cart object that need to be updated</param>
        /// <returns>Action result</returns>
        // PUT: api/Carts/5
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Cart>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutCart(Guid id, App.Public.DTO.v1.Cart cart)
        {
            if (id != cart.Id)
            {
                return BadRequest();
            }

            cart.AppUserId = User.GetUserId();

            if (cart.CartProducts != null) cart.TotalPrice = cart.CartProducts.Sum(c => c.Total);
            cart.TotalPrice = Math.Round(cart.TotalPrice, 2);
            cart.TotalPriceIncludingVat = Math.Round(cart.TotalPrice * 1.2m, 2);

            var bllCart = _mapper.Map(cart);
            _bll.CartService.Update(bllCart!);

            await _bll.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Create new Cart
        /// </summary>
        /// <param name="cart">New Cart object</param>
        /// <returns></returns>
        // POST: api/Carts
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Cart>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<App.Public.DTO.v1.Cart>> PostCart(App.Public.DTO.v1.Cart cart)
        {
            cart.AppUserId = User.GetUserId();

            if (cart.CartProducts != null) cart.TotalPrice = cart.CartProducts.Sum(c => c.Total);

            cart.TotalPrice = Math.Round(cart.TotalPrice, 2);
            cart.TotalPriceIncludingVat = Math.Round(cart.TotalPrice * 1.2m, 2);
            cart.Id = Guid.NewGuid();

            var bllCart = _mapper.Map(cart);

            _bll.CartService.Add(bllCart!);

            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
        }


        /// <summary>
        /// Delete Cart with specified id
        /// </summary>
        /// <param name="id">Cart ID</param>
        /// <returns>Action result</returns>
        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Cart>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            var cart = await _bll.CartService.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            await ClearCartAndDeleteCartProducts(cart);

            await _bll.CartService.RemoveAsync(cart.Id);

            await _bll.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Delete CartProducts with specified cart id
        /// </summary>
        /// <param name="id">Cart ID</param>
        /// <returns>Action result</returns>
        // DELETE: api/Carts/5/cartProducts
        [HttpDelete("{id}/cartProducts")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Cart>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCartProducts(Guid id)
        {
            var cart = await _bll.CartService.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            var cartProducts = _bll.CartProductService
                .AllAsync().Result
                .Where(cp => cp.CartId == cart.Id);
            foreach (var cartProduct in cartProducts)
            {
                await _bll.CartProductService.RemoveAsync(cartProduct.Id);
            }

            cart.TotalPrice = 0;
            cart.TotalPriceIncludingVat = 0;
            cart.CartProducts = new List<CartProduct>();

            await _bll.SaveChangesAsync();

            return NoContent();
        }


        private bool CartExists(Guid id)
        {
            return (_bll.CartService.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }


#pragma warning disable
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete]
        public async Task ClearCartAndDeleteCartProducts(App.BLL.DTO.Cart cart)
        {
            // Delete cartProducts
            var cartProducts = _bll.CartProductService.AllAsync().Result.Where(cp => cp.CartId == cart.Id);
            foreach (var cartProduct in cartProducts)
            {
                await _bll.CartProductService.RemoveAsync(cartProduct.Id);
            }

            await _bll.SaveChangesAsync();
        }
    }
}