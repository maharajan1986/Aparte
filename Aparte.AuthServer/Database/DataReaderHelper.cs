using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.WebApi
{
    /// <summary>
    /// DbDataReader does not support nullable type
    /// Help to get nullable type variable pulled from DataReader
    /// </summary>
    public static class DataReaderHelper
    {
        /// <summary>
        /// Get Nullable Int64 type value from DbDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static long? GetNullableInt64(this DbDataReader reader, int order)
        {
            if (reader.IsDBNull(order))
                return null;
            else
                return reader.GetInt64(order);
        }
        /// <summary>
        /// Get Nullable string type value from DbDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string GetNullableString(this DbDataReader reader, int order)
        {
            if (reader.IsDBNull(order))
                return null;
            else
                return reader.GetString(order);
        }
        /// <summary>
        /// Get Nullable DateTime value from DbDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static DateTime GetNullableDate(this DbDataReader reader, int order)
        {
            if (reader.IsDBNull(order))
                return new DateTime();
            else
                return reader.GetDateTime(order);
        }
    }
}
