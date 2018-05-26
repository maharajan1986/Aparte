using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.Models
{
    public class Tenant : Base
    {
        public string UniqueID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
