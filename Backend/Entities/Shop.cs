using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        //public int ProductId { get; set; }
        //public virtual Product Product { get; set; }
        public virtual List<ProductShop> ProductShops { get; set; }
    }
}
