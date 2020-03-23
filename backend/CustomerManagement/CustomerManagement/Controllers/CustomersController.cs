using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Data;
using CustomerManagement.Domain;
using CustomerManagement.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    //[Authorize]

    public class CustomersController : Controller
    {
        private ICustomersRepository _CustomersRepo;
        private ILogger _Logger;

        public CustomersController(ICustomersRepository customersRepository, ILogger<CustomersController> logger)
        {
            _CustomersRepo = customersRepository;
            _Logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerContext>),200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> Customers()
        {
            try
            {
                var customers = await _CustomersRepo.GetCustomersAsync();
                return Ok(customers);
            }
            catch(Exception ex)
            {
                _Logger.LogError(ex.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
            
        }


        [HttpGet("{id}",Name="GetCustomerRoute")]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> Customers(int id)
        {
            try
            {
                var customer = await _CustomersRepo.GetCustomerAsync(id);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return BadRequest(new ApiResponse { Status = false });
            }

        }


        [HttpPost]
        [ProducesResponseType(typeof(Customer),201)]
        [ProducesResponseType(typeof(ApiResponse),400)]
        public async Task<IActionResult> CreateCustomer ([FromBody] Customer customer)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse { Status = false, ModelState = ModelState });
 
            }
            try
            {
                var newCustomer = await _CustomersRepo.InsertCustomerAsync(customer);
                if(newCustomer == null)
                {
                    return BadRequest(new ApiResponse { Status = false });
                }
                return CreatedAtRoute("GetCustomerRoute", new { id = newCustomer.Id }, new ApiResponse { Status = true, Customer = newCustomer });

            }
            catch(Exception ex)
            {
                _Logger.LogError(ex.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> UpdateCustomer([FromBody]  Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse { Status = false, ModelState = ModelState });
            }
            try
            {
                var state = new State() { Id = customer.StateId };
                customer.State = state;
                var result = await _CustomersRepo.UpdateCustomerAsync(customer);
                if (!result)
                {
                    return BadRequest(new ApiResponse { Status = false });

                }
                return Ok(new ApiResponse { Status = true, Customer = customer });
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {

            try
            {

                var result = await _CustomersRepo.DeleteCustomerAsync(id);
                if (!result)
                {
                    return BadRequest(new ApiResponse { Status = false });

                }
                return Ok(new ApiResponse { Status = true });
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }
    }
}