using Microsoft.EntityFrameworkCore;
using PizzaApi.Domain.Models;

namespace PizzaApi.Infrastructure.Contexts
{ 
    public class ApiContext : DbContext
    {
        public DbSet<Pizza> PizzaMenu { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) :base(options)
        {
            
        }
    }
}
