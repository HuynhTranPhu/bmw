using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ViewModel
{
    public class ProductsViewModel
    {
        public Products Products { get; set; }
  
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<ProductCategory> ProductCategory { get; set; }


        public IEnumerable<Size> Size { get; set; }
        public IEnumerable<Color> Color { get; set; }


    }
}
