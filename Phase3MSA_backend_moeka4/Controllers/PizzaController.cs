using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaApi.Domain.Models;
using PizzaApi.Infrastructure.Contexts;

namespace PizzaApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly ApiContext _context;

        public PizzaController(ApiContext context)
        {
            _context = context;
        }

        // Post
        [HttpPost]
        public async Task<ActionResult<Pizza>> Create(Pizza pizza)
        {
            _context.PizzaMenu.Add(pizza);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = pizza.Id }, pizza);
        }

        // Put
        [HttpPut]
        public async Task<ActionResult<Pizza>> Edit(Pizza pizza)
        {
            var pizzaInDb = await _context.PizzaMenu.FirstOrDefaultAsync(p => p.Id == pizza.Id);
            if (pizzaInDb == null)
            {
                return NotFound();
            }

            _context.Entry(pizzaInDb).State = EntityState.Modified;

            pizzaInDb.Name = pizza.Name;
            pizzaInDb.IsVegan = pizza.IsVegan;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }


        // Get
        [HttpGet("{id}")]
        public async Task<ActionResult<Pizza>> Get(int id)
        {
            var result = await _context.PizzaMenu.FindAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.PizzaMenu == null)
            {
                return NotFound();
            }
            var result = await _context.PizzaMenu.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            _context.PizzaMenu.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Get all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pizza>>> GetAll()
        {
            if (_context.PizzaMenu == null)
            {
                return NotFound();
            }
            return await _context.PizzaMenu.ToListAsync();
        }
    }
}