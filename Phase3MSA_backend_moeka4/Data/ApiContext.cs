using Microsoft.EntityFrameworkCore;
using Phase3MSA_backend_moeka4.Models;

namespace Phase3MSA_backend_moeka4.Data
{ 
    public class ApiContext : DbContext
    {
        public DbSet<Pizza> PizzaMenu { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) :base(options)
        {
            
        }
    }
}
