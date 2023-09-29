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
using Payment = App.Domain.Payment;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Payments Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly PaymentMapper _mapper;


        /// <summary>
        /// Payments Constructor
        /// </summary>
        /// <param name="bll">Unit Of Work Interface</param>
        /// <param name="autoMapper">Auto Mapper</param>
        public PaymentsController(IAppBLL bll, IMapper autoMapper)
        {
            _bll = bll;
            _mapper = new PaymentMapper(autoMapper);
        }


        /// <summary>
        /// Get list of Payments
        /// </summary>
        /// <returns>List of all Payments</returns>
        // GET: api/Payments
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Payment>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Payment>>> GetPayments()
        {
            var vm = await _bll.PaymentService.AllAsync();
            var res = vm.Select(e => _mapper.Map(e))
                .ToList();
            return Ok(res);
            
            
            
        }


        /// <summary>
        /// Get Payment by Payment ID
        /// </summary>
        /// <param name="id">Payment ID</param>
        /// <returns>Payment object</returns>
        // GET: api/Payments/5
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Payment>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<App.Public.DTO.v1.Payment>> GetPayment(Guid id)
        {
            var payment = await _bll.PaymentService.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(payment);

            return Ok(res);
        }


        /// <summary>
        /// Update Payment with specified id
        /// </summary>
        /// <param name="id">Payment ID</param>
        /// <param name="payment">Payment object that need to be updated</param>
        /// <returns>Action result</returns>
        // PUT: api/Payments/5
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Payment>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutPayment(Guid id, App.Public.DTO.v1.Payment payment)
        {
            if (id != payment.Id)
            {
                return BadRequest();
            }

            var bllPayment = _mapper.Map(payment);
            _bll.PaymentService.Update(bllPayment!);

            await _bll.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Create new Payment
        /// </summary>
        /// <param name="payment">New Payment object</param>
        /// <returns>Created Payment object</returns>
        // POST: api/Payments
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Payment>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<App.Public.DTO.v1.Payment>> PostPayment(App.Public.DTO.v1.Payment payment)
        {
            payment.Id = Guid.NewGuid();
            
            var bllPayment = _mapper.Map(payment);
            _bll.PaymentService.Add(bllPayment!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
        }


        /// <summary>
        /// Delete Payment with specified id
        /// </summary>
        /// <param name="id">Payment ID</param>
        /// <returns>Action result</returns>
        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Payment>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            var payment = await _bll.PaymentService.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _bll.PaymentService.Remove(payment);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(Guid id)
        {
            return (_bll.PaymentService.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}