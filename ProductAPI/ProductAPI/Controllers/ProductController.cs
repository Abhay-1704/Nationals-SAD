using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductController : Controller
    {
       ProductDbContext db = new ProductDbContext();
        [HttpGet]
        public ActionResult<List<Product>> GetProducts()
        {
            var products = db.Products.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Image = x.Image
            }).ToList();
            if (!products.Any()){
                return NotFound("No Product to Show");
            }
            return Ok(products);
        }

        [HttpGet("{categoryId}")]
        public ActionResult<List<Product>> GetProductsbyCategory(int categoryId)
        {
            // put categoryId == 0 to show all products if categoryId is 0 then no need of above api to show all products
            var products = db.Products.Where(x => categoryId == 0 || x.CategoryId == categoryId).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Image = x.Image
            }).ToList();
            if (!products.Any())
            {
                return NotFound("No Product to Show");
            }
            return Ok(products);
        }

        [HttpGet("search")]
        //parameter determined by variable name in query string
        public ActionResult<List<Product>> SearchProductsbyName(String searchterm)
        {
            var products = db.Products.Where(x => x.Name.Contains(searchterm)).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Image = x.Image
            }).ToList();
            if (!products.Any())
            {
                return NotFound("No Product to Show");
            }
            return Ok(products);
        }

        [HttpPost]
        public ActionResult<Product> InsertProduct([FromBody] Product product)
        {
            try
            {
                db.Products.Add(product);
                db.SaveChanges();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        public ActionResult<Product> DeleteProduct(int productId)
        {
            try
            {
                Product product = db.Products.Where(x => x.Id == productId).FirstOrDefault();
                db.Products.Remove(product);
                db.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{productId}")]
        public ActionResult<Product> UpdateProduct(int productId, [FromBody] Product product)
        {
            try
            {
                Product updateproduct = db.Products.Where(x => x.Id == productId).FirstOrDefault();
                updateproduct.Name = product.Name;
                updateproduct.Description = product.Description;
                updateproduct.Image = product.Image;
                updateproduct.CategoryId = product.CategoryId;
                updateproduct.Price = product.Price;
                db.SaveChanges();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
