using System.Text.Json;
using CustomerAPI.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace CustomerAPI.Client
{
    /// <summary>
    /// Console application to demonstrate consuming the REST API Customer Service
    /// Converted from WCF client to HTTP REST client
    /// </summary>
    class Program
    {
        private static readonly string BaseUrl = "http://localhost:5202/api/customers";
        private static HttpClient? _httpClient;

        static async Task Main(string[] args)
        {
            // Create HTTP client
            var serviceProvider = new ServiceCollection()
                .AddHttpClient()
                .BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            _httpClient = httpClientFactory.CreateClient();

            try
            {
                Console.WriteLine("===============================================");
                Console.WriteLine("REST API Demo Client - .NET Core 8");
                Console.WriteLine("===============================================");
                Console.WriteLine();

                Console.WriteLine("Connecting to REST API Customer Service...");
                Console.WriteLine($"Base URL: {BaseUrl}");
                Console.WriteLine();

                // Demonstrate various REST API operations
                await DemonstrateRestApiOperations();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("ERROR: Could not connect to the REST API service.");
                Console.WriteLine("Make sure the CustomerAPI application is running first.");
                Console.WriteLine($"Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            finally
            {
                _httpClient?.Dispose();
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates various REST API operations
        /// </summary>
        static async Task DemonstrateRestApiOperations()
        {
            Console.WriteLine("=== REST API Service Operations Demo ===");
            Console.WriteLine();

            // 1. Get customer count
            Console.WriteLine("1. Getting total customer count...");
            var count = await GetCustomerCountAsync();
            Console.WriteLine($"   Total customers: {count}");
            Console.WriteLine();

            // 2. Get all customers
            Console.WriteLine("2. Getting all customers...");
            var customers = await GetAllCustomersAsync();
            Console.WriteLine($"   Found {customers.Count} customers:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"   - {customer.FirstName} {customer.LastName} ({customer.Email}) from {customer.City}, {customer.Country}");
            }
            Console.WriteLine();

            // 3. Get customer by ID
            Console.WriteLine("3. Getting customer by ID (ID = 3)...");
            var specificCustomer = await GetCustomerByIdAsync(3);
            if (specificCustomer != null)
            {
                Console.WriteLine($"   Found: {specificCustomer.FirstName} {specificCustomer.LastName}, born {specificCustomer.DateOfBirth:yyyy-MM-dd}");
            }
            else
            {
                Console.WriteLine("   Customer not found.");
            }
            Console.WriteLine();

            // 4. Get customers by country
            Console.WriteLine("4. Getting customers from USA...");
            var usaCustomers = await GetCustomersByCountryAsync("USA");
            Console.WriteLine($"   Found {usaCustomers.Count} customers from USA:");
            foreach (var customer in usaCustomers)
            {
                Console.WriteLine($"   - {customer.FirstName} {customer.LastName} from {customer.City}");
            }
            Console.WriteLine();

            // 5. Add a new customer
            Console.WriteLine("5. Adding a new customer...");
            var newCustomer = new Customer
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test.user@demo.com",
                DateOfBirth = new DateTime(1995, 1, 1),
                City = "Demo City",
                Country = "Demo Country"
            };
            var createdCustomer = await AddCustomerAsync(newCustomer);
            if (createdCustomer != null)
            {
                Console.WriteLine($"   New customer added with ID: {createdCustomer.Id}");
                Console.WriteLine();

                // 6. Update customer count after addition
                Console.WriteLine("6. Getting updated customer count...");
                count = await GetCustomerCountAsync();
                Console.WriteLine($"   Total customers after addition: {count}");
                Console.WriteLine();

                // 7. Update the customer
                Console.WriteLine("7. Updating the new customer...");
                createdCustomer.City = "Updated City";
                var updateResult = await UpdateCustomerAsync(createdCustomer);
                Console.WriteLine($"   Update result: {(updateResult ? "Success" : "Failed")}");
                Console.WriteLine();

                // 8. Verify the update
                Console.WriteLine("8. Verifying the update...");
                var updatedCustomer = await GetCustomerByIdAsync(createdCustomer.Id);
                if (updatedCustomer != null)
                {
                    Console.WriteLine($"   Updated customer: {updatedCustomer.FirstName} {updatedCustomer.LastName} from {updatedCustomer.City}");
                }
                Console.WriteLine();

                // 9. Delete the customer
                Console.WriteLine("9. Deleting the test customer...");
                var deleteResult = await DeleteCustomerAsync(createdCustomer.Id);
                Console.WriteLine($"   Delete result: {(deleteResult ? "Success" : "Failed")}");
                Console.WriteLine();

                // 10. Final customer count
                Console.WriteLine("10. Final customer count...");
                count = await GetCustomerCountAsync();
                Console.WriteLine($"    Total customers after deletion: {count}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("   Failed to create customer.");
            }

            Console.WriteLine("=== Demo Complete ===");
        }

        static async Task<List<Customer>> GetAllCustomersAsync()
        {
            var response = await _httpClient!.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Customer>>(json, GetJsonOptions()) ?? new List<Customer>();
        }

        static async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient!.GetAsync($"{BaseUrl}/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;
                
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Customer>(json, GetJsonOptions());
            }
            catch
            {
                return null;
            }
        }

        static async Task<List<Customer>> GetCustomersByCountryAsync(string country)
        {
            var response = await _httpClient!.GetAsync($"{BaseUrl}/country/{country}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Customer>>(json, GetJsonOptions()) ?? new List<Customer>();
        }

        static async Task<int> GetCustomerCountAsync()
        {
            var response = await _httpClient!.GetAsync($"{BaseUrl}/count");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(json, GetJsonOptions());
        }

        static async Task<Customer?> AddCustomerAsync(Customer customer)
        {
            var json = JsonSerializer.Serialize(customer, GetJsonOptions());
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient!.PostAsync(BaseUrl, content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Customer>(responseJson, GetJsonOptions());
        }

        static async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            var json = JsonSerializer.Serialize(customer, GetJsonOptions());
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            try
            {
                var response = await _httpClient!.PutAsync($"{BaseUrl}/{customer.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        static async Task<bool> DeleteCustomerAsync(int id)
        {
            try
            {
                var response = await _httpClient!.DeleteAsync($"{BaseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }
    }
}
