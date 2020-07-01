using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ViewModel
{
    public class AppointmentDetailsViewModel
    {
        public Order Order { get; set; }
        public List<User> SalesPerson { get; set; }
        public List<Products> Products { get; set; }
    }
}
