using Microsoft.AspNetCore.Mvc;
using CustomerAPI.Models;
using CustomerAPI.Services;

namespace CustomerAPI.Controllers
{
    /// <summary>
    /// REST API Controller for customer operations
    /// Converted from WCF service to REST endpoints with JSON responses
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <returns>List of all active customers</returns>
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        /// <summary>
        /// Gets a customer by ID
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>Customer object if found</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found");
            }
            return Ok(customer);
        }

        /// <summary>
        /// Gets customers by country
        /// </summary>
        /// <param name="country">The country name</param>
        /// <returns>List of customers from the specified country</returns>
        [HttpGet("country/{country}")]
        public async Task<ActionResult<List<Customer>>> GetCustomersByCountry(string country)
        {
            var customers = await _customerService.GetCustomersByCountryAsync(country);
            return Ok(customers);
        }

        /// <summary>
        /// Gets the total number of active customers
        /// </summary>
        /// <returns>Total customer count</returns>
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCustomerCount()
        {
            var count = await _customerService.GetCustomerCountAsync();
            return Ok(count);
        }

        /// <summary>
        /// Adds a new customer
        /// </summary>
        /// <param name="customer">The customer to add</param>
        /// <returns>The newly created customer with assigned ID</returns>
        [HttpPost]
        public async Task<ActionResult<Customer>> AddCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newId = await _customerService.AddCustomerAsync(customer);
            if (newId <= 0)
            {
                return BadRequest("Failed to create customer");
            }

            customer.Id = newId;
            return CreatedAtAction(nameof(GetCustomerById), new { id = newId }, customer);
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="id">The customer ID to update</param>
        /// <param name="customer">The customer with updated information</param>
        /// <returns>Updated customer</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest("ID in URL does not match ID in request body");
            }

            var success = await _customerService.UpdateCustomerAsync(customer);
            if (!success)
            {
                return NotFound($"Customer with ID {id} not found");
            }

            return Ok(customer);
        }

        /// <summary>
        /// Deletes a customer by ID (soft delete)
        /// </summary>
        /// <param name="id">The customer ID to delete</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var success = await _customerService.DeleteCustomerAsync(id);
            if (!success)
            {
                return NotFound($"Customer with ID {id} not found");
            }

            return NoContent();
        }
    }
}