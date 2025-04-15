﻿using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext context;
        public ProductsController(StoreContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await context.Products.ToListAsync();
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            Console.WriteLine("Hit");
            var product = await context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return product;
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return product;
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExist(id)) return BadRequest("Can't update this product");

            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var pro = await context.Products.FindAsync(id);
            if (pro == null) return NotFound();
            context.Products.Remove(pro);
            await context.SaveChangesAsync();
            return NoContent();
        }
        public bool ProductExist(int id)
        {
            return context.Products.Any(x => x.Id == id);
        }
    }
}
