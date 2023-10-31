using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Web_153501_Kiselev.Domain.Entities
{
    public class Vehicle : BaseEntities
    {
        public string? Model { get; set; }
        public string? Description { get; set; }
        public VehicleType? Type { get; set; }
        public decimal? Price { get; set; }
        public string? ImagePath { get; set; }
        public string? MimeType { get; set; }

    }
}
