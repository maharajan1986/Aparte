using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.Common.Sp
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterAttribute : System.Attribute
    {
        public ParameterAttribute()
        {
            this.Direction = ParameterDirection.Input;
        }
        public ParameterAttribute(int order)
        {
            this.Order = order;
        }
        public ParameterAttribute(int order, ParameterDirection direction, SqlDbType type, int size)
        {
            this.Order = order;
            this.SqlDbType = type;
            this.Direction = direction;
            this.Size = size;
        }

        public ParameterAttribute(int order, ParameterDirection direction, SqlDbType type)
        {

            this.Order = order;
            this.SqlDbType = type;
            this.Direction = direction;
        }

        public ParameterAttribute(int order, ParameterDirection direction, SqlDbType type, string typeName)
        {
            this.Order = order;
            this.SqlDbType = type;
            this.Direction = direction;
            this.TypeName = typeName;
        }
        public ParameterAttribute(int order, ParameterDirection direction)
        {
            this.Order = order;
            this.Direction = direction;
        }

        public ParameterAttribute(int order, ParameterDirection direction, int size)
        {
            this.Order = order;
            this.Direction = direction;
            this.Size = size;
        }
        public ParameterDirection Direction { get; set; }
        public int? Size { get; set; }
        public int Order { get; set; }
        public SqlDbType SqlDbType { get; set; }
        /// <summary>
        /// Used for Table-Value Parameter. Table Type name
        /// </summary>
        public string TypeName { get; set; }
    }
}
