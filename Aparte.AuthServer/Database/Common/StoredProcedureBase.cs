using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.Common.Sp
{
    public interface IStoredProcedureBase
    {
        string StoredProcedureName { get; set; }

    }
    /// <summary>
    /// All Stored Procedures should be called using Stored Procedure Package Class inherited from this base class
    /// Those Sp Package Class contain
    /// 1. Full Stored Procedure Name with schema such as tenant.GetTenant
    /// 2. All the parameters required to call the stored procedure, with the order of the parameters defined in the custom method attribute "Parameter"
    /// </summary>
    public abstract class StoredProcedureBase : BaseProperties, IStoredProcedureBase
    {
        protected StoredProcedureBase() { }

        /// <summary>
        /// Constructor of StoredProcedureBase class with a full stored procedure name existing in database
        /// </summary>
        /// <param name="name"></param>
        protected StoredProcedureBase(string name)
        {
            this.StoredProcedureName = name;
        }


        /// <summary>
        /// Full StoredProcedure Name in database with a schema name
        /// </summary>
        public string StoredProcedureName { get; set; }

        /// <summary>
        /// Execute Stored Procedure typically save.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public void Execute(DbContext context)
        {
            try
            {
                context.ExecuteStoredCommand(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Execute Stored Procedure and the result into Context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<T> Execute<T>(DbContext context) where T : Models.Base
        {
            try
            {
                return context.SqlQuery<T>(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        /// <summary>
        /// Execute Stored Procedure and return independent resultset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteView<T>(DbContext context)
        {
            try
            {
                context.Database.CommandTimeout = 60 * 50; // 50 Mins
                return context.Database.SqlQuery<T>(this);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return null;
        }

        public static object GetReaderValue(System.Data.Common.DbDataReader reader, string name)
        {
            var ordinal = reader.GetOrdinal(name);
            if (reader.IsDBNull(ordinal))
                return null;
            try
            {
                return reader.GetValue(ordinal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public static long? GetReaderValueInt64(System.Data.Common.DbDataReader reader, string name)
        {
            var ordinal = reader.GetOrdinal(name);
            if (reader.IsDBNull(ordinal))
                return null;
            try
            {
                return reader.GetInt64(ordinal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public static DateTime? GetReaderValueDate(System.Data.Common.DbDataReader reader, string name)
        {
            var ordinal = reader.GetOrdinal(name);
            if (reader.IsDBNull(ordinal))
                return null;
            try
            {
                return reader.GetDateTime(ordinal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public static string GetReaderValueString(System.Data.Common.DbDataReader reader, string name)
        {
            var ordinal = reader.GetOrdinal(name);
            if (reader.IsDBNull(ordinal))
                return null;
            try
            {
                return reader.GetString(ordinal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}
