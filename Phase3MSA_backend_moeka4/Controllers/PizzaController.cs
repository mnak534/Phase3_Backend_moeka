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
        /// <summary>
        /// Create a new pizza on the menu
        /// </summary>
        /// <param name="pizza">A pizza to add to the menu (Please enter 0 for the ID)</param>
        /// <returns></returns>
        /// <response code="201">Pizza created</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<Pizza>> Create(Pizza pizza)
        {
            if (pizza.Id != 0)
            {
                return BadRequest("Please enter 0 for an ID");
            }
            //_context.addPizza(pizza);
            _context.PizzaMenu.Add(pizza);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = pizza.Id }, pizza);
        }

        // Put
        /// <summary>
        /// Edit a pizza that is already on the menu
        /// </summary>
        /// <param name="pizza">Pizza to be modified (ID must stay the same)</param>
        /// <returns></returns>
        /// <response code="201">Pizza modified</response>
        [HttpPut]
        [ProducesResponseType(201)]
        public async Task<ActionResult<Pizza>> Edit(Pizza pizza)
        {
            var pizzaInDb = await _context.PizzaMenu.FirstOrDefaultAsync(p => p.Id == pizza.Id);
            if (pizzaInDb == null)
            {
                return BadRequest("No such pizza is on the menu");
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

            return CreatedAtAction("Get", new { id = pizza.Id }, pizza);
        }


        // Get
        /// <summary>
        /// Find a pizza
        /// </summary>
        /// <param name="id">ID of the pizza to find</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Pizza>> Get(int id)
        {
            var result = await _context.PizzaMenu.FindAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // Delete
        /// <summary>
        /// Remove a pizza from the menu
        /// </summary>
        /// <param name="id">ID of the pizza to delete</param>
        /// <returns></returns>
        /// <response code="204">Product successfully deleted</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
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
        /// <summary>
        /// Get a list of all the pizzas on the menu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
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