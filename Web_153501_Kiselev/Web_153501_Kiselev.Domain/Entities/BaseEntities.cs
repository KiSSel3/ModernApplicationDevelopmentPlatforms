using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_153501_Kiselev.Domain.Entities
{
    public class BaseEntities
    {
        [Key]
        public Guid Id { get; set; }
    }
}
