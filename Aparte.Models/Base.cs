using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aparte.Models
{
    public class Base
    {
        [Key]
        public long PK { get; set; }
        public virtual string GlobalID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime? Inserted { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime? LastModified { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime? Deleted { get; set; }

        // Concurrent Update Control (Optimistic Lock)
        [Timestamp]
        public virtual Byte[] Timestamp { get; set; }
    }
}
