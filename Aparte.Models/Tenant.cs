using System;
using System.ComponentModel.DataAnnotations;

namespace Aparte.Models
{
    public class Tenant : Base
    {
        public string Code { get; set; }

        [Required(ErrorMessage ="Name field is required."), StringLength(10, ErrorMessage="Cannot insert more than 10 character")]
        public string Name { get; set; }

    }
    public class TenantMenu
    {
        [Key]
        public virtual long? PK { get; set; }
        public virtual string Uri { get; set; }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        public virtual string Caption { get; set; }

        public virtual long? FKParent { get; set; }

        public byte[] Contents { get; set; }

        public double? No { get; set; }

        public string GroupCode { get; set; }
    }
}
