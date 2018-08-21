using System;
namespace Aparte.Models
{
    public class TenantUser : Base
    {
        public virtual long? FKTenant { get; set; }
        public virtual long? FKSystemUser { get; set; }
        public virtual long? FKParty { get; set; }
        public virtual DateTime Registered { get; set; }
    }
}
