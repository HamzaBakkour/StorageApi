using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.Data;
using StorageApi.Models;
using StorageApi.DTOs;
using StorageApi.Migrations;

namespace StorageApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StorageApiContext _context;

        public ProductsController(StorageApiContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProduct([FromQuery] string? category)
        {
            IQueryable<Product> query = _context.Product;

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category.ToLower() == category.ToLower());
            }

            var products = await query.ToListAsync();

            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Count = p.Count
            }).ToList();

            return Ok(productDtos);
        }

        // GET: api/Products/stats
        [HttpGet("stats")]
        public async Task<ActionResult<ProductStatsDto>> GetProduct(int id)
        {
            var products = await _context.Product.ToListAsync();

            var totalCount = products.Sum(p => p.Count);
            var totalInventoryValue = products.Sum(p => p.Count * p.Price);
            var averagePrice = products.Average(p => p.Price);

            var stats = new ProductStatsDto
            {
                TotalCount = totalCount,
                TotalInventoryValue = totalInventoryValue,
                AveragePrice = Math.Round(averagePrice, 2)
            };

            return Ok(stats);

        }




        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, CreateProductDto requestBody)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
                return NotFound();

            product.Name = requestBody.Name;
            product.Price = requestBody.Price;
            product.Category = requestBody.Category;
            product.Shelf = requestBody.Shelf;
            product.Count = requestBody.Count;
            product.Description = requestBody.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(CreateProductDto requestBody)
        {


            var product = new Product
            {
                Name = requestBody.Name,
                Price = requestBody.Price,
                Category = requestBody.Category,
                Shelf = requestBody.Shelf,
                Count = requestBody.Count,
                Description = requestBody.Description
            };


            _context.Product.Add(product);
            await _context.SaveChangesAsync();


            var dto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Count = product.Count
            };

            return CreatedAtAction("GetProduct", new { id = product.Id }, dto);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
                return NotFound();


            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
