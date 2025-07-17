using CustomerAPI.Models;

namespace CustomerAPI.Services
{
    /// <summary>
    /// Interface for customer service operations
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Gets all active customers
        /// </summary>
        /// <returns>List of all active customers</returns>
        Task<List<Customer>> GetAllCustomersAsync();

        /// <summary>
        /// Gets a customer by ID
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>Customer object if found, null otherwise</returns>
        Task<Customer?> GetCustomerByIdAsync(int customerId);

        /// <summary>
        /// Gets customers by country
        /// </summary>
        /// <param name="country">The country name</param>
        /// <returns>List of customers from the specified country</returns>
        Task<List<Customer>> GetCustomersByCountryAsync(string country);

        /// <summary>
        /// Adds a new customer
        /// </summary>
        /// <param name="customer">The customer to add</param>
        /// <returns>The ID of the newly created customer</returns>
        Task<int> AddCustomerAsync(Customer customer);

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="customer">The customer with updated information</param>
        /// <returns>True if update was successful, false otherwise</returns>
        Task<bool> UpdateCustomerAsync(Customer customer);

        /// <summary>
        /// Deletes a customer by ID (soft delete)
        /// </summary>
        /// <param name="customerId">The customer ID to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteCustomerAsync(int customerId);

        /// <summary>
        /// Gets the total number of active customers
        /// </summary>
        /// <returns>Total active customer count</returns>
        Task<int> GetCustomerCountAsync();
    }
}