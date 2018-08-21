using Aparete.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.AuthServer
{
    public class User
    {
        public long? PKSystemUser { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryInUtc { get; set; }
        public ulong ExpiryInUlong
        {
            get { return JWT.GetExpiryDuration(ExpiryInUtc); }
        }
        public string ExpiryInString
        {
            get { return ExpiryInUlong.ToString(); }
        }
    }
    public static class UserList
    {
        static Dictionary<long?, User> users = new Dictionary<long?, User>();
        public static string GetAccessToken(long? pkSytemUser)
        {
            if (users.ContainsKey(pkSytemUser))
            {
                var user = users[pkSytemUser];
                return user.AccessToken;
            }
            return string.Empty;
        }
        public static string GetUserCode(long? pkSytemUser)
        {
            if (users.ContainsKey(pkSytemUser))
            {
                var user = users[pkSytemUser];
                return user.UserCode;
            }
            return string.Empty;
        }
        public static User Add(long? pkSystemUser, string userName, string userCode, Guid accessToken, Guid refreshToken, DateTime expiry)
        {
            return Add(pkSystemUser, userName, userCode, accessToken.ToString(), refreshToken.ToString(), expiry);
        }

        public static User Add(long? pkSystemUser, string userName, string userCode, string accessToken, string refreshToken, DateTime expiry)
        {
            User user = null;
            if (users.ContainsKey(pkSystemUser))
                user = users[pkSystemUser];
            else
                user = new User();
            user.PKSystemUser = pkSystemUser;
            user.UserName = userName;
            user.UserCode = userCode;
            user.AccessToken = accessToken;
            user.RefreshToken = refreshToken;
            user.ExpiryInUtc = expiry;
            if (!users.ContainsKey(pkSystemUser))
                users.Add(pkSystemUser, user);
            return user;
        }

        public static bool Contains(long? pkSystemUser)
        {
            return users.ContainsKey(pkSystemUser);
        }

        public static DateTime? GetTokenExpirationDateTimeInUtc(long? pkSystemUser)
        {
            var user = users.ContainsKey(pkSystemUser) ? users[pkSystemUser] : null;
            if (user != null)
            {
                return user.ExpiryInUtc;
            }
            return null;
        }

        public static bool IsTokenExpired(long? pkSystemUser)
        {
            var user = users.ContainsKey(pkSystemUser) ? users[pkSystemUser] : null;
            if (user != null)
            {
                return JWT.IsTokenExpired(user.ExpiryInUlong);
            }
            return true;
        }

        public static string Refresh(long? pkSystemUser, string refreshToken)
        {
            var user = users[pkSystemUser];
            if (user != null)
            {
                if (user.RefreshToken == refreshToken)
                {
                    return user.AccessToken;
                }
            }
            return string.Empty;
        }
    }
}
