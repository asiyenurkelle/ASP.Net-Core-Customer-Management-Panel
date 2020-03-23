using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Data
{
    public class CustomersRepository : ICustomersRepository
    {
        private CustomerContext _Context;
        private ILogger _Logger;
        public CustomersRepository(CustomerContext Context, ILoggerFactory loggerFactory)
        {
            _Context = Context;
            _Logger = loggerFactory.CreateLogger("CustomersRepository");

        }


        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _Context.Customers

                            .Include(s => s.State)
                            .Include(o => o.Orders)
                            .SingleOrDefaultAsync(c => c.Id == id);
        }
        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _Context.Customers
                             .OrderBy(o => o.LastName)
                             .Include(s => s.State)
                             .Include(o => o.Orders)
                             .ToListAsync();
        }


        public async Task<Customer> InsertCustomerAsync(Customer customer)
        {
            _Context.Add(customer);
            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _Logger.LogError($"Error in {nameof(InsertCustomerAsync)} :" + ex.Message);
            }
            return customer;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            _Context.Customers.Attach(customer);
            _Context.Entry(customer).State = EntityState.Modified;
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Error in {nameof(UpdateCustomerAsync)} :" + ex.Message);
            }
            return false;

        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _Context.Customers
                   .Include(o => o.Orders)
                   .SingleOrDefaultAsync(c => c.Id == id);
            _Context.Remove(customer);
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Error in {nameof(DeleteCustomerAsync)} :" + ex.Message);
            }
            return false;
        }

        
    }
}
