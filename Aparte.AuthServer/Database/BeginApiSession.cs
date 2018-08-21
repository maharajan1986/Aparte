using Aparte.Common.Sp;
using Aparte.Models;
using Aparte.Repository;
using Aparte.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.WebApi
{
    /// <summary>
    /// Stored Procedure Definition related to the user validation and begin api session
    /// </summary>
    public class BeginApiSession : Common.Sp.StoredProcedureBase
    {
        /// <summary>
        /// Static Authentication method to validate userId and password, returning SystemUser information
        /// This method does not use BeginApiSession stored procedure object, instead reading the system user matching the user code
        /// because password hash is calculated in C#, we cannot match password in database
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns>SystemUser</returns>
        public static SystemUser ValidatePassword(ApiContext context, string userId, string password)
        {
            try
            {
                var sysUser = context.SystemUsers.FirstOrDefault(x => x.Code == userId);
                if (sysUser == null)
                    return null;
                var hashedPassword = new Password(password, sysUser.PasswordSalt);
                if (hashedPassword.DerivedKey.SequenceEqual(sysUser.PasswordKey))
                    return sysUser;
            }
            catch (Exception e)
            {

            }
            return null;
        }
        /// <summary>
        /// Stored Procedure Call to begin Api Session, passing the pkSystemUser obtained from ValidatePassword method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pkSystemUser"></param>
        /// <returns>ApiSession</returns>
        public static ApiSession Execute(ApiContext context, long? pkSystemUser)
        {
            try
            {
                var sp = new BeginApiSession(pkSystemUser);
                var reader = context.GetReader(sp);
                if (reader.Read())
                {
                    var user = new ApiSession();
                    user.PKSystemUser = reader.GetInt64(0);
                    user.UserId = reader.GetString(1);
                    user.UserName = reader.GetString(2);
                    user.UserCode = reader.GetString(3);
                    user.AccessGranted = reader.GetDateTime(4);
                    user.AccessToken = reader.GetGuid(5);
                    user.RefreshToken = reader.GetGuid(6);
                    user.TokenExpiry = reader.GetDateTime(7);
                    return user;
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return null;
        }
        /// <summary>
        /// Constructor with PK System User
        /// This class cannot instantiated without first valide the user and get PKSystemUser
        /// </summary>
        /// <param name="pkSystemUser"></param>
        public BeginApiSession(long? pkSystemUser)
            : base("tenant.Authenticate")
        {
            this.PKSystemUser = pkSystemUser;
        }

        /// <summary>
        /// Parameter to call this stored procedure
        /// </summary>
        [Parameter(0, ParameterDirection.Input, SqlDbType.BigInt)]
        public long? PKSystemUser { get; set; }

    }
}
