using Backend.Contexts;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class CategoryService
    {
        private readonly WebShopContext _db = new WebShopContext();
        public IQueryable<CategoryModel> GetQueryCategory()
        {
            return _db.Categories.Select(c => new CategoryModel
            {
                Id = c.Id,
                Name = c.Name
            });
        }
    }
}
