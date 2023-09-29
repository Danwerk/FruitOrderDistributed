using System.Net.Mime;
using App.Contracts.BLL;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Public.DTO.Mappers;
using App.Public.DTO.v1;
using Asp.Versioning;
using AutoMapper;
using DAL.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Unit = App.Domain.Unit;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Units Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UnitMapper _mapper;

        /// <summary>
        /// Units Constructor
        /// </summary>
        /// <param name="bll">Unit Of Work Interface</param>
        /// <param name="autoMapper">Auto Mapper</param>
        public UnitsController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new UnitMapper(autoMapper);
        }


        /// <summary>
        /// Get list of Units
        /// </summary>
        /// <returns>List of all Units</returns>
        // GET: api/Units
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Unit>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Unit>>> GetUnits()
        {
            var vm = await _bll.UnitService.AllAsync();
            var res = vm.Select(e => _mapper.Map(e))
                .ToList();
            return Ok(res);
        }


        /// <summary>
        /// Get Unit by Unit ID
        /// </summary>
        /// <param name="id">Unit ID</param>
        /// <returns>Unit object</returns>
        // GET: api/Units/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Unit>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<App.Public.DTO.v1.Unit>> GetUnit(Guid id)
        {
            var unit = await _bll.UnitService.FindAsync(id);

            if (unit == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(unit);
            return Ok(res);
        }


        /// <summary>
        /// Update Unit with specified id
        /// </summary>
        /// <param name="id">Unit ID</param>
        /// <param name="unit">Unit object that need to be updated</param>
        /// <returns>Action result</returns>
        // PUT: api/Units/5
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Unit>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutUnit(Guid id, [FromBody] App.Public.DTO.v1.Unit unit)
        {
            if (id != unit.Id)
            {
                return BadRequest();
            }

            var bllUnit = _mapper.Map(unit);
            _bll.UnitService.Update(bllUnit!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Create new Unit
        /// </summary>
        /// <param name="unit">New Unit object</param>
        /// <returns>Created Unit object</returns>
        // POST: api/Units
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Unit>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Unit>> PostUnit(App.Public.DTO.v1.Unit unit)
        {
            unit.Id = Guid.NewGuid();
            var bllUnit = _mapper.Map(unit);
            _bll.UnitService.Add(bllUnit!);
            await _bll.SaveChangesAsync();


            return CreatedAtAction("GetUnit", new { id = unit.Id }, unit);
        }

        /// <summary>
        /// Delete Unit with specified id
        /// </summary>
        /// <param name="id">Unit ID</param>
        /// <returns>Action result</returns>
        // DELETE: api/Units/5
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Unit>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUnit(Guid id)
        {
            var unit = await _bll.UnitService.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }

            await _bll.UnitService.RemoveAsync(unit.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool UnitExists(Guid id)
        {
            return (_bll.UnitService.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}