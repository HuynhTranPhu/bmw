using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ViewModel
{
    public class AppointmentViewModel
    {
           public List<Order> Order { get; set; }

           public Page Page { get; set; }
    }
}
