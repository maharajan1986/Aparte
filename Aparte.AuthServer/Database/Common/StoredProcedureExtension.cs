using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.Common.Sp
{
    public static class StoredProcedureExtension
    {
        public static List<T> GetList<T>(this DbContext self, IStoredProcedureBase sp) where T : class
        {
            var reader = GetReader(self, sp);
            var objectContext = ((IObjectContextAdapter)self).ObjectContext;
            var list = objectContext.Translate<T>(reader).ToList();
            reader.Close();
            reader.Dispose();
            return list;
        }

        public static System.Data.Common.DbDataReader GetReader(this DbContext self, IStoredProcedureBase sp)
        {
            string parmarray = "";
            try
            {
                if (self == null)
                    throw new ArgumentNullException("self");
                if (sp == null)
                    throw new ArgumentException("StoredProcedure Extension parameter sp is null");
                if (string.IsNullOrEmpty(sp.StoredProcedureName))
                    throw new ArgumentException("StoredProcedure Name is not defined");

                var cmd = self.Database.Connection.CreateCommand();
                cmd.CommandTimeout = 60 * 50; // 50 Mins
                cmd.CommandText = sp.StoredProcedureName;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                var arguments = PrepareArguments(sp);

                foreach (var param in arguments.Item2)
                {
                    cmd.Parameters.Add(param);
                    parmarray = parmarray + string.Format("{0}={1},", param.ParameterName, param.Value);
                }
                if (((IObjectContextAdapter)self).ObjectContext.Connection.State != System.Data.ConnectionState.Closed)
                    ((IObjectContextAdapter)self).ObjectContext.Connection.Close();
                ((IObjectContextAdapter)self).ObjectContext.Connection.Open();

                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Restart();
                var _cmd = cmd;
                var reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                stopWatch.Stop();
                                                
                return reader;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        public static int ExecuteStoredCommand(this DbContext self, IStoredProcedureBase sp)
        {
            string parmarray = "";
            if (self == null)
                throw new ArgumentNullException("self");
            if (sp == null)
                throw new ArgumentException("StoredProcedure Extension parameter sp is null");
            if (string.IsNullOrEmpty(sp.StoredProcedureName))
                throw new ArgumentException("StoredProcedure Name is not defined");

            var arguments = PrepareArguments(sp);
            var objectContext = ((IObjectContextAdapter)self).ObjectContext;
            objectContext.CommandTimeout = 60 * 50; // 50 mins
            try
            {
                foreach (var param in arguments.Item2)
                {
                    parmarray = parmarray + string.Format("{0}={1},", param.ParameterName, param.Value);
                }
                var noOfRowsEffected = objectContext.ExecuteStoreCommand("execute " + arguments.Item1, arguments.Item2);
                FeedBackOutputParameter(sp, arguments.Item2);

                return noOfRowsEffected;
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

        private static void FeedBackOutputParameter(IStoredProcedureBase sp, SqlParameter[] parameters)
        {
            foreach (var p in parameters)
            {
                if (p.Direction == System.Data.ParameterDirection.Output || p.Direction == System.Data.ParameterDirection.InputOutput)
                {
                    var name = p.ParameterName;
                    foreach (PropertyInfo propertyInfo in sp.GetType().GetProperties())
                    {
                        var attributes = propertyInfo.GetCustomAttributes(typeof(ParameterAttribute), true);
                        if (attributes.Length == 0)
                            continue;
                        if (name == "@" + propertyInfo.Name)
                        {
                            propertyInfo.SetValue(sp, p.Value, null);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Wrapper function of Context.Set<T>().SqlQuery
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static IEnumerable<T> SqlQuery<T>(this DbContext self, IStoredProcedureBase sp) where T : class
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (sp == null)
                throw new ArgumentException("StoredProcedure Extension parameter sp is null");
            var arguments = PrepareArguments(sp);
            try
            {
                return self.Set<T>().SqlQuery(arguments.Item1, arguments.Item2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;

        }

        /// <summary>
        /// Wrapper function of Database.SqlQuery<T>.
        /// T can be any view, not necessary predefined model class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static IEnumerable<T> SqlQuery<T>(this System.Data.Entity.Database self, IStoredProcedureBase sp)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (sp == null)
                throw new ArgumentException("IStoredProcedure sp");
            var arguments = PrepareArguments(sp);
            try
            {
                return self.SqlQuery<T>(arguments.Item1, arguments.Item2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public static Tuple<string, SqlParameter[]> PrepareArguments(IStoredProcedureBase sp)
        {
            if (string.IsNullOrEmpty(sp.StoredProcedureName))
                throw new Exception("Gene.EF.Common.Sp: Stored Procedure Name is empty!");
            var spName = sp.StoredProcedureName;
            var parameterNames = new Dictionary<int, string>();
            var parameterParameters = new Dictionary<int, SqlParameter>();

            foreach (PropertyInfo propertyInfo in sp.GetType().GetProperties())
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(ParameterAttribute), true);
                if (attributes.Length == 0)
                    continue;

                string name = "@" + propertyInfo.Name;

                object value = propertyInfo.GetValue(sp, null);
                var a = (ParameterAttribute)attributes[0];

                var param = new SqlParameter();
                param.ParameterName = name;
                if (value == null)
                    param.SqlDbType = a.SqlDbType;
                param.Value = value ?? DBNull.Value;
                param.Direction = a.Direction;
                param.TypeName = param.SqlDbType == System.Data.SqlDbType.Structured ? a.TypeName : param.TypeName;
                if (a.Size.HasValue)
                    param.Size = a.Size.Value;

                name += param.Direction == System.Data.ParameterDirection.Output ? " out" : "";

                parameterNames.Add(a.Order, name);
                parameterParameters.Add(a.Order, param);
            }

            var names = new List<string>();
            var parameters = new List<SqlParameter>();
            for (var i = 0; i < parameterNames.Count; i++)
            {
                names.Add(parameterNames[i]);
                parameters.Add(parameterParameters[i]);
            }

            if (parameterNames.Count > 0)
                spName = String.Format("{0} {1}", sp.StoredProcedureName, string.Join(", ", names.ToArray()));

            return new Tuple<string, SqlParameter[]>(spName, parameters.ToArray());
        }
    }
}
