using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? UnitPrice { get; set; }
        public int? Stock { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<ProductShop> ProductShops { get; set; }
    }
}
