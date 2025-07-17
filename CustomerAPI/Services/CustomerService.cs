using CustomerAPI.Models;

namespace CustomerAPI.Services
{
    /// <summary>
    /// Implementation of the Customer Service with fake data
    /// Ported from the original WCF service to work with REST API
    /// </summary>
    public class CustomerService : ICustomerService
    {
        // In-memory fake data storage
        private static readonly List<Customer> _customers = new();
        private static int _nextId = 1;

        static CustomerService()
        {
            InitializeFakeData();
        }

        /// <summary>
        /// Initialize fake customer data
        /// </summary>
        private static void InitializeFakeData()
        {
            _customers.AddRange(new List<Customer>
            {
                new Customer(1, "John", "Doe", "john.doe@email.com", new DateTime(1985, 3, 15), "New York", "USA"),
                new Customer(2, "Jane", "Smith", "jane.smith@email.com", new DateTime(1990, 7, 22), "London", "UK"),
                new Customer(3, "Carlos", "Rodriguez", "carlos.rodriguez@email.com", new DateTime(1988, 11, 8), "Madrid", "Spain"),
                new Customer(4, "Marie", "Dubois", "marie.dubois@email.com", new DateTime(1992, 5, 3), "Paris", "France"),
                new Customer(5, "Hans", "Mueller", "hans.mueller@email.com", new DateTime(1983, 12, 30), "Berlin", "Germany"),
                new Customer(6, "Anna", "Kowalski", "anna.kowalski@email.com", new DateTime(1989, 9, 14), "Warsaw", "Poland"),
                new Customer(7, "Luigi", "Rossi", "luigi.rossi@email.com", new DateTime(1987, 4, 18), "Rome", "Italy"),
                new Customer(8, "Sarah", "Johnson", "sarah.johnson@email.com", new DateTime(1993, 2, 25), "Toronto", "Canada"),
                new Customer(9, "Yuki", "Tanaka", "yuki.tanaka@email.com", new DateTime(1991, 8, 11), "Tokyo", "Japan"),
                new Customer(10, "Pedro", "Silva", "pedro.silva@email.com", new DateTime(1986, 6, 7), "São Paulo", "Brazil")
            });
            _nextId = 11;
        }

        public Task<List<Customer>> GetAllCustomersAsync()
        {
            return Task.FromResult(_customers.Where(c => c.IsActive).ToList());
        }

        public Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return Task.FromResult(_customers.FirstOrDefault(c => c.Id == customerId && c.IsActive));
        }

        public Task<List<Customer>> GetCustomersByCountryAsync(string country)
        {
            if (string.IsNullOrEmpty(country))
                return Task.FromResult(new List<Customer>());

            return Task.FromResult(_customers.Where(c => c.IsActive && 
                string.Equals(c.Country, country, StringComparison.OrdinalIgnoreCase)).ToList());
        }

        public Task<int> AddCustomerAsync(Customer customer)
        {
            if (customer == null)
                return Task.FromResult(-1);

            customer.Id = _nextId++;
            customer.IsActive = true;
            _customers.Add(customer);
            return Task.FromResult(customer.Id);
        }

        public Task<bool> UpdateCustomerAsync(Customer customer)
        {
            if (customer == null)
                return Task.FromResult(false);

            var existingCustomer = _customers.FirstOrDefault(c => c.Id == customer.Id);
            if (existingCustomer == null)
                return Task.FromResult(false);

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Email = customer.Email;
            existingCustomer.DateOfBirth = customer.DateOfBirth;
            existingCustomer.City = customer.City;
            existingCustomer.Country = customer.Country;
            existingCustomer.IsActive = customer.IsActive;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
                return Task.FromResult(false);

            // Soft delete - just mark as inactive
            customer.IsActive = false;
            return Task.FromResult(true);
        }

        public Task<int> GetCustomerCountAsync()
        {
            return Task.FromResult(_customers.Count(c => c.IsActive));
        }
    }
}