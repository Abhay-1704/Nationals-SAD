using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    //on which this class in accessible
    [Route("api/[Controller]")]
    [ApiController]
    public class Category : Controller
    {
        ProductDbContext db = new ProductDbContext();

        [HttpGet]
        public ActionResult<List<ProductAPI.Models.Category>> GetCategories()
        {
            try
            {
                List<ProductAPI.Models.Category> categories = db.Categories.ToList();
                if (!categories.Any())
                {
                    return NotFound("No categories found.");
                }
                ProductAPI.Models.Category cat = new ProductAPI.Models.Category();
                cat.Id = 0;
                cat.Name = "All Categories";
                categories.Insert(0, cat);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}
