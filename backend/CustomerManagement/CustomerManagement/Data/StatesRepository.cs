 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Data
{
    public class StatesRepository : IStatesRepository
    {
        private CustomerContext _Context;
        private ILogger _Logger;
        public StatesRepository(CustomerContext Context, ILoggerFactory loggerFactory)
        {
            _Context = Context;
            _Logger = loggerFactory.CreateLogger("StatesRepository");
        }
        public async Task<List<State>> GetStatesAsync()
        {
            return await _Context.States.OrderBy(c => c.Abbreviation).ToListAsync();
        }
    }
}
