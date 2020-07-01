using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Size
    {
        public int SizeID { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
