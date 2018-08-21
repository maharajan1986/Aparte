using Aparte.Common.Sp;
using Aparte.Repository;
using Aparte.WebApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.WebApi
{
    /// <summary>
    /// Stored Procedure Package Class to call tenant.GetTenant in database
    /// </summary>
    public class GetTenant : Common.Sp.StoredProcedureBase
    {
        /// <summary>
        /// Update ApiSession by calling the Stored Procedure with PKSystemUser in session
        /// This is called after a user successfully log in and begin ApiSession
        /// </summary>
        /// <param name="context"></param>
        /// <param name="session"></param>
        public static void Execute(ApiContext context, ApiSession session)
        {
            try
            {
                var sp = new GetTenant(session.PKSystemUser);
                var reader = context.GetReader(sp);
                while (reader.Read())
                {
                    var tu = new TenantUserInfo();
                    tu.PKTenant = reader.GetInt64(0);
                    tu.TenantCode = reader.GetNullableString(1);
                    tu.TenantName = reader.GetNullableString(2);
                    tu.PKTenantUser = reader.GetInt64(3);
                    tu.PKParty = reader.GetNullableInt64(4);
                    tu.PartyCode = reader.GetNullableString(5);
                    tu.PartyName = reader.GetNullableString(6);
                    session.Tenants.Add(tu);
                }
            }
            catch
            {
                throw;
            }

        }
        private GetTenant(long? pkSystemUser) : base("tenant.GetTenant")
        {
            this.PKSystemUser = pkSystemUser;
        }

        /// <summary>
        /// Stored Procedure Parameter, holding PK for SystemUser
        /// </summary>
        [Parameter(0, ParameterDirection.Input, SqlDbType.NVarChar)]
        public long? PKSystemUser { get; set; }

    }
}
