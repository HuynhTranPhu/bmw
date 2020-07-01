using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ViewModel
{
    public class ShoppingCartViewModel
    {
        public List<Products> Products { get; set; }
        public Order Order { get; set; }
        
    }
}
