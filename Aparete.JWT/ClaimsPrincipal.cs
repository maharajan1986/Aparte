using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Aparete.JWT
{
    /// <summary>
    /// User Principal class, inheriting from ClaimsPrincipal
    /// Purpose : used to store user , pksystem user, fktenant
    /// </summary>
    public class UserPrincipal : ClaimsPrincipal
    {
        Claim pkSystemUser = null;
        Claim fkTenant = null;
        public UserPrincipal(IEnumerable<ClaimsIdentity> identities) : base(identities)
        {
        }
        /// <summary>
        /// Paramter less constructor
        /// </summary>
        public UserPrincipal()
        {

        }
        public UserPrincipal(IIdentity identity) : base(identity) { }
        /// <summary>
        /// constructor with Json Web Token
        /// </summary>
        /// <param name="jwt"></param>
        public UserPrincipal(JWT jwt)
        {
            var identity = this.GetIdentity();
            if (jwt.Claims.ContainsKey(JWTConstant.PK_SYSTEM_USER))
            {
                pkSystemUser = new Claim("PKSystemUser", jwt.Claims[JWTConstant.PK_SYSTEM_USER]);
                identity.AddClaim(pkSystemUser);
            }
            if (jwt.Claims.ContainsKey(JWTConstant.FK_TENANT))
            {
                fkTenant = new Claim("FKTenant", jwt.Claims[JWTConstant.FK_TENANT]);
                identity.AddClaim(fkTenant);
            }
        }

        private ClaimsIdentity GetIdentity()
        {
            return base.Identity as ClaimsIdentity;
        }
    }
}
