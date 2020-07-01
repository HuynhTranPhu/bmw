using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }

        public int AppointmentId { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
