using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.Public.DTO.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Public.DTO.v1;
using Asp.Versioning;
using AutoMapper;
using DAL.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Discount = App.Domain.Discount;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Discounts Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DiscountsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DiscountMapper _mapper;


        /// <summary>
        /// Discounts Constructor
        /// </summary>
        /// <param name="bll">Unit Of Work Interface</param>
        /// <param name="autoMapper">Auto Mapper</param>
        public DiscountsController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new DiscountMapper(autoMapper);
        }


        /// <summary>
        /// Get list of Discounts
        /// </summary>
        /// <returns>List of all discounts</returns>
        // GET: api/Discounts
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Discount>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Discount>>> GetDiscounts()
        {
            var vm = await _bll.DiscountService.AllAsync();
            var res = vm.Select(e => _mapper.Map(e))
                .ToList();

            return Ok(res);
        }


        /// <summary>
        /// Get Discount by Discount ID
        /// </summary>
        /// <param name="id">Discount ID</param>
        /// <returns>Discount object</returns>
        // GET: api/Discounts/5
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Discount>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<App.Public.DTO.v1.Discount>> GetDiscount(Guid id)
        {
            var discount = await _bll.DiscountService.FindAsync(id);

            if (discount == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(discount);
            return Ok(res);
        }


        /// <summary>
        /// Update Discount with specified id
        /// </summary>
        /// <param name="id">Discount ID</param>
        /// <param name="discount">Discount object that need to be updated</param>
        /// <returns>Action result</returns>
        // PUT: api/Discounts/5
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Discount>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutDiscount(Guid id, App.Public.DTO.v1.Discount discount)
        {
            if (id != discount.Id)
            {
                return BadRequest();
            }

            var bllDiscount = _mapper.Map(discount);
            _bll.DiscountService.Update(bllDiscount!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Create new discount
        /// </summary>
        /// <param name="discount">New Discount object</param>
        /// <returns>Created Discount object</returns>
        // POST: api/Discounts
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Discount>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<App.Public.DTO.v1.Discount>> PostDiscount(App.Public.DTO.v1.Discount discount)
        {
            var bllDiscount = _mapper.Map(discount);
            _bll.DiscountService.Add(bllDiscount!);

            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetDiscount", new { id = discount.Id }, discount);
        }


        /// <summary>
        /// Delete Discount with specified id
        /// </summary>
        /// <param name="id">Discount ID</param>
        /// <returns>Action result</returns>
        // DELETE: api/Discounts/5
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Discount>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteDiscount(Guid id)
        {
            var discount = await _bll.DiscountService.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            await _bll.DiscountService.RemoveAsync(discount.Id);

            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool DiscountExists(Guid id)
        {
            return (_bll.DiscountService.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}