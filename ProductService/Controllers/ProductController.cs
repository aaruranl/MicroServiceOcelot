using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Database;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _productDbContext;

        public ProductController(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productDbContext.Products.ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] Product productRequest)
        {
            await _productDbContext.Products.AddAsync(productRequest);
            await _productDbContext.SaveChangesAsync();
            return Ok(productRequest);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            var products = await _productDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromBody] Product updateRequest)
        {
            var products = await _productDbContext.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            products.ProductName = updateRequest.ProductName;
            products.ProductDescription = updateRequest.ProductDescription;
            products.ProductPrice = updateRequest.ProductPrice;
            products.ProductStock = updateRequest.ProductStock;

            await _productDbContext.SaveChangesAsync();

            return Ok(products);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var products = await _productDbContext.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            _productDbContext.Products.Remove(products);
            await _productDbContext.SaveChangesAsync();
            return Ok(products);
        }
        
    }
}
