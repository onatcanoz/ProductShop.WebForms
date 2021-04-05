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
    public class ShopService
    {
        private readonly WebShopContext _db = new WebShopContext();

        public IQueryable<ShopModel> GetQueryShop()
        {
            return _db.Shops.Select(s => new ShopModel()
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address
            });
        }

        public void Add(ShopModel model)
        {
            Shop entity = new Shop()
            {
                Name = model.Name,
                Address = model.Address
            };
            _db.Shops.Add(entity);
            _db.SaveChanges();
        }

        public void Update(ShopModel model)
        {
            Shop shop = _db.Shops.Find(model.Id);
            shop.Name = model.Name;
            shop.Address = model.Address;
            _db.Entry(shop).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            Shop entity = _db.Shops.Find(id);
            if(entity.ProductShops.Count == 0)
            {
                _db.Shops.Remove(entity);
                _db.SaveChanges();
            }           
        }
    }
}
