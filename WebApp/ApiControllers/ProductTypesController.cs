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
using App.Domain.Identity;
using App.Public.DTO.Mappers;
using App.Public.DTO.v1;
using Asp.Versioning;
using AutoMapper;
using DAL.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ProductType = App.Domain.ProductType;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// ProductTypes Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly ProductTypeMapper _mapper;


        /// <summary>
        /// ProductTypes Constructor
        /// </summary>
        /// <param name="bll">Unit Of Work Interface</param>
        /// <param name="autoMapper">Auto Mapper</param>
        public ProductTypesController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new ProductTypeMapper(autoMapper);
        }


        /// <summary>
        /// Get list of ProductTypes
        /// </summary>
        /// <returns>List of all productTypes</returns>
        // GET: api/ProductTypes
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.ProductType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.ProductType>>> GetProductTypes()
        {
            var vm = await _bll.ProductTypeService.AllAsync();

            var res = vm.Select(e => _mapper.Map(e))
                .ToList();

            return Ok(res);
        }


        /// <summary>
        /// Get ProductType by ProductType ID
        /// </summary>
        /// <param name="id">ProductType ID</param>
        /// <returns>ProductType object</returns>
        // GET: api/ProductTypes/5
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.ProductType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<App.Public.DTO.v1.ProductType>> GetProductType(Guid id)
        {
            var productType = await _bll.ProductTypeService.FindAsync(id);

            if (productType == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(productType);

            return Ok(res);
        }


        /// <summary>
        /// Update ProductType with specified id
        /// </summary>
        /// <param name="id">ProductType ID</param>
        /// <param name="productType">ProductType object that need to be updated</param>
        /// <returns>Action result</returns>
        // PUT: api/ProductTypes/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.ProductType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutProductType(Guid id, App.Public.DTO.v1.ProductType productType)
        {
            if (id != productType.Id)
            {
                return BadRequest();
            }

            var bllProductType = _mapper.Map(productType);
            _bll.ProductTypeService.Update(bllProductType!);

            await _bll.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Create new ProductType
        /// </summary>
        /// <param name="productType">New ProductType object</param>
        /// <returns>Created ProductType object</returns>
        // POST: api/ProductTypes
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.ProductType>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<App.Public.DTO.v1.ProductType>> PostProductType(
            App.Public.DTO.v1.ProductType productType)
        {
            var bllProductType = _mapper.Map(productType);
            _bll.ProductTypeService.Add(bllProductType!);

            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetProductType", new { id = productType.Id }, productType);
        }


        /// <summary>
        /// Delete ProductType with specified id
        /// </summary>
        /// <param name="id">ProductType ID</param>
        /// <returns>Action result</returns>
        // DELETE: api/ProductTypes/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.ProductType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProductType(Guid id)
        {
            var productType = await _bll.ProductTypeService.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            await _bll.ProductTypeService.RemoveAsync(productType.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductTypeExists(Guid id)
        {
            return (_bll.ProductTypeService.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}