using Microsoft.EntityFrameworkCore;
using PizzaApi.Domain.Models;

namespace PizzaApi.Infrastructure.Contexts
{ 
    public class ApiContext : DbContext
    {
        int nextID =  1;

        public DbSet<Pizza> PizzaMenu { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) :base(options)
        {
            
        }

        public void addPizza(Pizza pizza)
        {
            pizza.Id = nextID;
            Pizza newPizza = new Pizza
            {
                Id = nextID,
                Name = pizza.Name,
                IsVegan = pizza.IsVegan
            };
            PizzaMenu.Add(newPizza);
            nextID++;
        }
    }
}
