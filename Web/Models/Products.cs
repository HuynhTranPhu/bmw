using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal PromotionPrice { get; set; }
        public string Detail { get; set; }

        public int Quantity { get; set; }
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        [Display(Name = "Product Category")]
        public int ProductCategoryID { get; set; }

        [ForeignKey("ProductCategoryID")]
        public virtual ProductCategory ProductCategory { get; set; }
        [Display(Name = "Size")]
        public int SizeID { get; set; }

        [ForeignKey("SizeID")]
        public virtual Size Size { get; set; }
        [Display(Name = "Color")]
        public int ColorID { get; set; }

        [ForeignKey("ColorID")]
        public virtual Color Color { get; set; }

        public int Warranty { get; set; }

        public bool Status { get; set; }
        public string Image { get; set; }
    }
}
