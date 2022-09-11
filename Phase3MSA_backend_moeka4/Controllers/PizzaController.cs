using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phase3MSA_backend_moeka4.Models;
using Phase3MSA_backend_moeka4.Data;

namespace Phase3MSA_backend_moeka4.Controllers
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

        // Create or Edit
        [HttpPost]
        public JsonResult CreateEdit(Pizza pizza)
        {
            if(pizza.Id == 0)
            {
                _context.PizzaMenu.Add(pizza);
            }
            else
            {
                var pizzaInDb = _context.PizzaMenu.Find(pizza.Id);
                if (pizzaInDb == null)
                    return new JsonResult(NotFound());
                pizzaInDb = pizza;
            }

            _context.SaveChanges();
            return new JsonResult(Ok(pizza));
        }

        // Get
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _context.PizzaMenu.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }

        // Delete
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.PizzaMenu.Find(id);
            if (result == null)
                return new JsonResult(NotFound());

            _context.PizzaMenu.Remove(result);
            _context.SaveChanges();

            return new JsonResult(NoContent());
        }

        // Get all
        [HttpGet]
        public JsonResult GetAll()
        {
            var result = _context.PizzaMenu.ToList();

            return new JsonResult(Ok(result));
        }
    }
}
