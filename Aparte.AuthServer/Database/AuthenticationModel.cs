using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.WebApi
{
    /// <summary>
    /// Minimum set of user information when a user switches the tenant
    /// User authentication is the same for all the tenants = PKSystemUser
    /// but user property may be different in each tenant = PKParty, PKTenantUser
    /// </summary>
    public class TenantUserInfo
    {
        /// <summary>
        /// PKTenant - the unique key for each tenant
        /// </summary>
        public long PKTenant { get; set; }
        /// <summary>
        /// Code for the tenant, which is normally Project Code such as JobCode
        /// </summary>
        public string TenantCode { get; set; }
        /// <summary>
        /// Name for the tenant, which is normally Project Name such as Barzan Project
        /// </summary>
        public string TenantName { get; set; }
        /// <summary>
        /// Key for the user of the tenant (cross section of Tenant x SystemUser)
        /// </summary>
        public long PKTenantUser { get; set; }
        /// <summary>
        /// Key for the user in a certain tenant
        /// </summary>
        public long? PKParty { get; set; }
        /// <summary>
        /// Key for the user for all the tenants
        /// </summary>
        public long? PKSystemUser { get; set; }
        /// <summary>
        /// Code for the System User (common for all the tenants)
        /// </summary>
        public string SystemUserCode { get; set; }
        /// <summary>
        /// Name for the System User (common for all the tenants)
        /// </summary>
        public string SystemUserName { get; set; }
        /// <summary>
        /// Code for the user in the Party Table in one tenant
        /// </summary>
        public string PartyCode { get; set; }
        /// <summary>
        /// Name for the user in the Party Table in one tenant
        /// </summary>
        public string PartyName { get; set; }
    }
    /// <summary>
    /// The minimum set of system user information
    /// </summary>
    public class ApiSession
    {
        /// <summary>
        /// Constructor initializing the list of TenantUserInfo
        /// </summary>
        public ApiSession()
        {
            Tenants = new List<TenantUserInfo>();
        }
        /// <summary>
        /// Key for the System User (common for all the tenants)
        /// </summary>
        public long PKSystemUser { get; set; }
        /// <summary>
        /// Key of the user starting the Api session
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Name of the user starting the Api session
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Code of the user starting the Api session
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// Access Token the user receives
        /// </summary>
        public Guid AccessToken { get; set; }
        /// <summary>
        /// Refresh Token the user receives
        /// </summary>
        public Guid RefreshToken { get; set; }
        /// <summary>
        /// The date and time of the access grant
        /// </summary>
        public DateTime AccessGranted { get; set; }
        /// <summary>
        /// The date and time of the access token expiry
        /// </summary>
        public DateTime TokenExpiry { get; set; }
        /// <summary>
        /// The tenants(project) that the user can access
        /// </summary>
        public List<TenantUserInfo> Tenants { get; private set; }
    }
}
