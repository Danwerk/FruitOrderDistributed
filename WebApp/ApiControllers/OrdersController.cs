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

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Orders Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly OrderMapper _mapper;

        /// <summary>
        /// Orders Controller
        /// </summary>
        /// <param name="bll">Unit Of Work Interface</param>
        /// <param name="autoMapper">Auto Mapper</param>
        public OrdersController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new OrderMapper(autoMapper);
        }


        /// <summary>
        /// Get list of Orders
        /// </summary>
        /// <returns>List of all Orders</returns>
        // GET: api/Orders
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Order>>> GetOrders()
        {
            if (User.IsInRole("admin"))
            {
                var vm = await _bll.OrderService.AllAsync();

                var res = vm.Select(e => _mapper.Map(e))
                    .ToList();
                return Ok(res);
            }
            else
            {
                var vm = await _bll.OrderService.AllAsync(User.GetUserId());
                var res = vm.Select(e => _mapper.Map(e))
                    .ToList();
                return Ok(res);
            }
        }


        /// <summary>
        /// Get Order by Order ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order object</returns>
        // GET: api/Orders/5
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<App.Public.DTO.v1.Order>> GetOrder(Guid id)
        {
            if (User.IsInRole("admin"))
            {
                var vm = await _bll.OrderService.FindAsync(id);
                if (vm == null)
                {
                    return NotFound();
                }

                var res = _mapper.Map(vm);
                return Ok(res);
            }
            else
            {
                var vm = await _bll.OrderService.FindAsync(id, User.GetUserId());
                var res = _mapper.Map(vm);
                return Ok(res);
            }
        }


        /// <summary>
        /// Update Order with specified id
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="order">Order object that need to be updated</param>
        /// <returns>Action result</returns>
        // PUT: api/Orders/5
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutOrder(Guid id, App.Public.DTO.v1.Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            var bllOrder = _mapper.Map(order);
            bllOrder!.AppUserId = User.GetUserId();
            _bll.OrderService.Update(bllOrder);

            await _bll.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Create new Order
        /// </summary>
        /// <param name="order">New Order object</param>
        /// <returns>Created Order object</returns>
        // POST: api/Orders
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Order>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<App.Public.DTO.v1.Order>> PostOrder(App.Public.DTO.v1.Order order)
        {
            order.AppUserId = User.GetUserId();
            order.Id = Guid.NewGuid();

            var bllOrder = _mapper.Map(order);

            _bll.OrderService.Add(bllOrder!);

            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }


        /// <summary>
        /// Delete Order with specified id
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Action result</returns>
        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _bll.OrderService.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await DeleteOrderProducts(order);
            
            await _bll.OrderService.RemoveAsync(order.Id);

            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(Guid id)
        {
            return (_bll.OrderService.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        
        #pragma warning disable
        [HttpDelete]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task DeleteOrderProducts(App.BLL.DTO.Order order)
        {
            // Delete orderProducts
            var orderProducts = _bll.OrderProductService.AllAsync().Result.Where(cp => cp.OrderId == order.Id);
            foreach (var orderProduct in orderProducts)
            {
                await _bll.OrderProductService.RemoveAsync(orderProduct.Id);
            }

            await _bll.SaveChangesAsync();
        }
    }
}