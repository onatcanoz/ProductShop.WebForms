using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? UnitPrice { get; set; }
        public int? Stock { get; set; } 
        public int? CategoryId { get; set; }

        //custom
        public string Category { get; set; }
        public List<int> ShopIdleri { get; set; }
        public List<ShopModel> Shops { get; set; }
        public string ShopsText
        {
            get
            {
                return string.Join("<br />", Shops.Select(s => s.Name).ToList());
            }
        }

    }
}
