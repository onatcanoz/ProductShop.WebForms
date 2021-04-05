using Backend.Contexts;
using Backend.Entities;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class ProductService
    {
        private readonly WebShopContext _db = new WebShopContext();

        public IQueryable<ProductModel> GetQuery()
        {
            return _db.Products.Select(p => new ProductModel()
            {
                Id = p.Id,
                Name = p.Name,
                UnitPrice = p.UnitPrice,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                Category = p.Category.Name,
                ShopIdleri = p.ProductShops.Select(ps => ps.ShopId).ToList(),
                Shops = p.ProductShops.Select(pss => new ShopModel()
                {
                    Id = pss.Shop.Id,
                    Address = pss.Shop.Address,
                    Name = pss.Shop.Name
                }).ToList()
            }); 
        }

        public void Add(ProductModel model)
        {
            Product entity = new Product()
            {
                Name = model.Name,
                UnitPrice = model.UnitPrice,
                Stock = model.Stock,
                CategoryId = model.CategoryId.Value,
                ProductShops = model.ShopIdleri.Select(shopId => new ProductShop()
                {
                    ShopId = shopId
                }).ToList()
            };
            _db.Products.Add(entity);
            _db.SaveChanges();
        }

        public void Update(ProductModel model)
        {
            Product product = _db.Products.Find(model.Id);
            product.Name = model.Name;
            product.UnitPrice = model.UnitPrice;
            product.Stock = model.Stock;
            product.CategoryId = model.CategoryId.Value;

            _db.ProductShops.RemoveRange(product.ProductShops);
            product.ProductShops = model.ShopIdleri.Select(shopId => new ProductShop()
            {
                ProductId = product.Id,
                ShopId = shopId
            }).ToList();
            _db.Entry(product).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            Product entity = _db.Products.Find(id);
            _db.ProductShops.RemoveRange(entity.ProductShops);
            _db.Products.Remove(entity);
            _db.SaveChanges();
        }
    }
}
