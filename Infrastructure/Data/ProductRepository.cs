using Core.Entities;
using Core.Interfaces;
using Core.RequestHelpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProductRepository(StoreContext context) : IProductRepository
    {       
        public void AddProduct(Product product)
        {
            context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await context.Products.Select(x => x.Brand)
                .Distinct()
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<Pagination<Product>> GetProductsAsync(ProductSpecParams productSpecParams)
        {
            var query = context.Products.AsQueryable();
            if(!string.IsNullOrEmpty(productSpecParams.Search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(productSpecParams.Search));
            }
            if(productSpecParams.Brands.Count > 0)
            {
                query = query.Where(x => productSpecParams.Brands.Contains(x.Brand));
            }
            if(productSpecParams.Types.Count > 0)
            {
                query = query.Where(x => productSpecParams.Types.Contains(x.Type));
            }
            if(productSpecParams.sort != null)
            {
                query = productSpecParams.sort switch
                {
                    "priceAsc" => query.OrderBy(x => x.Price),
                    "priceDesc" => query.OrderByDescending(x => x.Price),
                    _ => query.OrderBy(x => x.Name)
                };
            }
            var products = await query
                .Skip((productSpecParams.PageIndex - 1) * productSpecParams.PageSize)
                .Take(productSpecParams.PageSize)
                .ToListAsync();
            var count = await query.CountAsync();
            var pagination = new Pagination<Product>(productSpecParams.PageIndex, productSpecParams.PageSize, count, products);
            return pagination;
        }

        public async Task<IReadOnlyList<string>> GetTypeAsync()
        {
            return await context.Products.Select(x => x.Type)
                .Distinct()
                .ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return context.Products.Any(x => x.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            context.Entry(product).State = EntityState.Modified;

        }
    }
}
