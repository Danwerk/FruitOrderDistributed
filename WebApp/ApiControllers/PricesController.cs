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
using DAL.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Price = App.Domain.Price;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Prices Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class PricesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly PriceMapper _mapper;


        /// <summary>
        /// Prices Constructor
        /// </summary>
        /// <param name="bll">Unit Of Work Interface</param>
        /// <param name="autoMapper">Auto Mapper</param>
        public PricesController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new PriceMapper(autoMapper);
        }

        
        /// <summary>
        /// Get list of Prices
        /// </summary>
        /// <returns>List of all Prices</returns>
        // GET: api/Prices
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Price>) , StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Price>>> GetPrices()
        {
            var vm = await _bll.PriceService.AllAsync();
            var res = vm.Select(e => _mapper.Map(e))
                .ToList();
            return Ok(res);
        }

        
        /// <summary>
        /// Get Price by Price ID
        /// </summary>
        /// <param name="id">Price ID</param>
        /// <returns>Price object</returns>
        // GET: api/Prices/5
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Price>) , StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse) ,StatusCodes.Status404NotFound)]
        public async Task<ActionResult<App.Public.DTO.v1.Price>> GetPrice(Guid id)
        {
            var price = await _bll.PriceService.FindAsync(id);

            if (price == null)
            {
                return NotFound();
            }
            var res = _mapper.Map(price);

            return Ok(res);
        }

        
        /// <summary>
        /// Update Price with specified id
        /// </summary>
        /// <param name="id">Price ID</param>
        /// <param name="price">Price object that need to be updated</param>
        /// <returns>Action result</returns>
        // PUT: api/Prices/5
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Price>) , StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse) ,StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse) ,StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutPrice(Guid id, App.Public.DTO.v1.Price price)
        {
            if (id != price.Id)
            {
                return BadRequest();
            }

            var bllPrice = _mapper.Map(price);
            _bll.PriceService.Update(bllPrice!);

            await _bll.SaveChangesAsync();

            return NoContent();
        }

        
        /// <summary>
        /// Create new Price
        /// </summary>
        /// <param name="price">New Price object</param>
        /// <returns>Created Price object</returns>
        // POST: api/Prices
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Price>) , StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse) ,StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<App.Public.DTO.v1.Price>> PostPrice(App.Public.DTO.v1.Price price)
        {
            var bllPrice = _mapper.Map(price);
            _bll.PriceService.Add(bllPrice!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetPrice", new { id = price.Id }, price);
        }

        
        /// <summary>
        /// Delete Price with specified id
        /// </summary>
        /// <param name="id">Price ID</param>
        /// <returns>Action result</returns>
        // DELETE: api/Prices/5
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Price>) , StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse) ,StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse) ,StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePrice(Guid id)
        {
            var price = await _bll.PriceService.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }

            await _bll.PriceService.RemoveAsync(price.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool PriceExists(Guid id)
        {
            return (_bll.PriceService.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
