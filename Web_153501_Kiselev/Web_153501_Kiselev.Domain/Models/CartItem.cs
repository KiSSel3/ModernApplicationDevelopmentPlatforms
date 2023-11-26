using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_153501_Kiselev.Domain.Entities;

namespace Web_153501_Kiselev.Domain.Models
{
    public class CartItem
    {
        public Vehicle Vehicle { get; set; }
        public int Count { get; set; }
    }
}
