using Aparte.Common.Sp;
using Aparte.Repository;
using System.Data;

namespace Aparte.WebApi
{
    /// <summary>
    /// Stored Procedure Package class, calling "tenant.RefreshApiSession"
    /// The difference between BeginApiSession and RefreshAbpSession is AccessToken and RefreshToken
    /// RefreshToken will be validated in database
    /// </summary>
    public class RefreshApiSession : Common.Sp.StoredProcedureBase
    {
        /// <summary>
        /// Get a new ApiSession by calling the stored procedure, with defined parameters
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pkSystemUser"></param>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public static ApiSession Execute(ApiContext context, long? pkSystemUser, string accessToken, string refreshToken)
        {
            try
            {
                var sp = new RefreshApiSession(pkSystemUser, accessToken, refreshToken);
                var reader = context.GetReader(sp);
                if (reader.Read())
                {
                    var user = new ApiSession();
                    user.PKSystemUser = reader.GetInt64(0);
                    user.UserId = reader.GetString(1);
                    user.UserName = reader.GetString(2);
                    user.AccessGranted = reader.GetDateTime(3);
                    user.AccessToken = reader.GetGuid(4);
                    user.RefreshToken = reader.GetGuid(5);
                    user.TokenExpiry = reader.GetDateTime(6);
                    return user;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }
        /// <summary>
        /// Constructore of RefreshApiSession
        /// </summary>
        /// <param name="pkSystemUser"></param>
        /// <param name="currentAcessToken"></param>
        /// <param name="refreshToken"></param>
        public RefreshApiSession(long? pkSystemUser, string currentAcessToken, string refreshToken) : base("tenant.RefreshApiSession")
        {
            this.PKSystemUser = pkSystemUser;
            this.CurrentAccessToken = currentAcessToken;
            this.RefreshToken = refreshToken;
        }
        /// <summary>
        /// First parameter PKsystemUser
        /// </summary>
        [Parameter(0, ParameterDirection.Input, SqlDbType.BigInt)]
        public long? PKSystemUser { get; set; }
        /// <summary>
        /// 2nd parameter CurrentAccessToken (GUID format)
        /// </summary>
        [Parameter(1, ParameterDirection.Input, SqlDbType.VarChar)]
        public string CurrentAccessToken { get; set; }
        /// <summary>
        /// 3rd parameter RefreshToken (GUID format)
        /// </summary>
        [Parameter(2, ParameterDirection.Input, SqlDbType.VarChar)]
        public string RefreshToken { get; set; }

    }
}
