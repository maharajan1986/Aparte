using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.Models
{
    public class Base
    {
        [Key]
        public long PK { get; set; }
        public DateTime? Inserted { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
