using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using App.Contracts.BLL;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Public.DTO.Mappers;
using App.Public.DTO.v1;
using Asp.Versioning;
using AutoMapper;
using Base.Helpers;
using DAL.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using OrderProduct = App.Domain.OrderProduct;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// OrderProducts Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderProductsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly OrderProductMapper _mapper;


        /// <summary>
        /// OrderProducts Controller
        /// </summary>
        /// <param name="bll">Unit Of Work Interface</param>
        /// <param name="autoMapper">Auto Mapper</param>
        public OrderProductsController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new OrderProductMapper(autoMapper);
        }

        /// <summary>
        /// Get list of OrderProducts
        /// </summary>
        /// <returns>List of all OrderProducts</returns>
        // GET: api/OrderProducts
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.OrderProduct>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.OrderProduct>>> GetOrderProducts()
        {
            if (User.IsInRole("admin"))
            {
                var vm = await _bll.OrderProductService.AllAsync();

                var res = vm.Select(e => _mapper.Map(e))
                    .ToList();
                return Ok(res);
            }
            else
            {
                var vm = await _bll.OrderProductService.AllAsync(User.GetUserId());
                var res = vm.Select(e => _mapper.Map(e))
                    .ToList();
                return Ok(res);
            }
        }


        /// <summary>
        /// Get OrderProduct by OrderProduct ID
        /// </summary>
        /// <param name="id">OrderProduct ID</param>
        /// <returns>OrderProduct object</returns>
        // GET: api/OrderProducts/5
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.OrderProduct>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<App.Public.DTO.v1.OrderProduct>> GetOrderProduct(Guid id)
        {
            var orderProduct = await _bll.OrderProductService.FindAsync(id);

            if (orderProduct == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(orderProduct);
            return Ok(res);
        }


        /// <summary>
        /// Update OrderProduct with specified id
        /// </summary>
        /// <param name="id">OrderProduct ID</param>
        /// <param name="orderProduct">OrderProduct object that need to be updated</param>
        /// <returns>Action result</returns>
        // PUT: api/OrderProducts/5
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.OrderProduct>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutOrderProduct(Guid id, App.Public.DTO.v1.OrderProduct orderProduct)
        {
            if (id != orderProduct.Id)
            {
                return BadRequest();
            }

            var bllOrderProduct = _mapper.Map(orderProduct);
            _bll.OrderProductService.Update(bllOrderProduct!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Create new OrderProduct
        /// </summary>
        /// <param name="orderProduct">New Discount object</param>
        /// <returns>Created OrderProduct object</returns>
        // POST: api/OrderProducts
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.OrderProduct>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<App.Public.DTO.v1.OrderProduct>> PostOrderProduct(
            App.Public.DTO.v1.OrderProduct orderProduct)
        {
            orderProduct.Id = Guid.NewGuid();
            
            var bllOrderProduct = _mapper.Map(orderProduct);
            _bll.OrderProductService.Add(bllOrderProduct!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetOrderProduct", new { id = orderProduct.Id }, orderProduct);
        }


        /// <summary>
        /// Delete OrderProduct with specified id
        /// </summary>
        /// <param name="id">OrderProduct ID</param>
        /// <returns>Action result</returns>
        // DELETE: api/OrderProducts/5
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.OrderProduct>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteOrderProduct(Guid id)
        {
            var orderProduct = await _bll.OrderProductService.FindAsync(id);
            if (orderProduct == null)
            {
                return NotFound();
            }

            _bll.OrderProductService.Remove(orderProduct);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderProductExists(Guid id)
        {
            return (_bll.OrderProductService.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}