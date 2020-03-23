using CustomerManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Data
{
    public interface IStatesRepository
    {
        Task<List<State>> GetStatesAsync();
    }
}
