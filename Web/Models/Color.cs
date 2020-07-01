using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Color
    {
        public int ColorID { get; set; }
        [Required]
        public string NameColor { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
