using CustomerManagement.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Data
{
    public class CustomerContext: IdentityDbContext<AppUser>
    {

        public CustomerContext(DbContextOptions<CustomerContext> options)
            :base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet <Order> Orders { get; set; }
        public DbSet <State> States { get; set; } 
    }
}
