using Aparete.JWT;
using Aparte.ApiClient.Aparte.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.ApiClient
{
    /// <summary>
    /// Static class to hold all the server and client interactio data.
    /// </summary>
    public static class WinClient
    {
        //TODO : find how to create these keys.
        public const string PUBLIC_KEY = "<RSAKeyValue><Modulus>noztbrZlpSsSlDS6qu8AhCsQgymdsCwEdcFt+xioTCQiJ0kQBoUMIib43WKSRvN7ky3EBJqiGGW7v0QTKc9L/qYrPqTW6U4npQuI6Fwm5ANaPxr/Cb2jZuKqJ0DhBkxApi547H/r+QZtNXpKa4TtaTvfqnaDdNHFyNuhsbxnp50=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public const string PRIVATE_KEY = "<RSAKeyValue><Modulus>noztbrZlpSsSlDS6qu8AhCsQgymdsCwEdcFt+xioTCQiJ0kQBoUMIib43WKSRvN7ky3EBJqiGGW7v0QTKc9L/qYrPqTW6U4npQuI6Fwm5ANaPxr/Cb2jZuKqJ0DhBkxApi547H/r+QZtNXpKa4TtaTvfqnaDdNHFyNuhsbxnp50=</Modulus><Exponent>AQAB</Exponent><P>zfPC2EMO+30QPbMWnaICPTxxoosMpE5x9gjbSbqXRHS9fJOiaVr6ylBOPAeO5pM5ZNZpvz/RDqjL+Oeddp8F+Q==</P><Q>xRRRWqr58NEXgzyWzPXffE4Hsm6yFWXf6lcOCYLJXMzRFn5UIBPxjYHKuTYE25UuXEqFvGHsBqhLwSv8/GBHxQ==</Q><DP>y0S6W04cPHTcEblvKdeblCrTERViPcy6x5VIMcW6xLLfzlO8KXXFLucBwFfJX6ORMwg4SK1Ivco1vw2CqXAcqQ==</DP><DQ>VLPJz2UirSzApUf6LDcUiXFj/31yDp5NYYNu5gmPD9J7nuZGs86+h6ob/gRIjDOOzF2/ItsXPTlB7dFBFxsuXQ==</DQ><InverseQ>S+aPSzW8mwwKQRXBQgpAaoQuEEow5A6Eg0cklPiJAeD+NU4ROR6uRcrNUhzvj/CAuqSDb/F6Fzb1Wk6NCAzd2g==</InverseQ><D>BiR5kl94oqR/jMRaMwMdZwFwG1TCai/aYGGFDERRSNnhhNocx9Phu3T9ET6fYiZOtE4CmRcQjpdqZaeSgn0oeX6DT0KofgDsdyErvI/dNX58kTHxxI07dvnmvG13eJSeW2DG5WKOR66ADePNspI2Ja+GsiHEGuUvNCmToq1SYKE=</D></RSAKeyValue>";
        // Move these Uri to confirg
        public static string ApiServiceUri { get; set; } = "http://localhost:58578/";
        public static string OData4ServiceUri { get; set; } = "http://localhost:58578/aparte";

        public static string ServerPublicKey { get; set; }
        public static bool IsClientAuthenticated { get; set; }
        public static bool IsClientAuthorized { get; set; }
        /// <summary>
        /// True when user is authenticated.
        /// </summary>
        public static bool IsUserAuthenticated { get; set; }

        //Get Access token
        public static string AccessToken { get; set; }
        public static string RefreshToken { get; set; }

        public static long? PKSystemUser { get; set; }
        public static string SystemUserName { get; set; }
        public static string SystemUserId { get; set; }

        public static long? PKPerson { get; set; }
        public static long? FKTenant { get; set; }

        //verify client
        public static string SharedSymmetricKey = "";        

        public static DateTime TokenExpiryInUtc { get; set; }
        public static ulong TokeyExpiryDuration
        {
            get { return JWT.GetExpiryDuration(TokenExpiryInUtc); }
        }
        public static bool IsTokenExpired
        {
            get { return JWT.IsTokenExpired(TokeyExpiryDuration); }
        }
        //public static Proxy Proxy { get; set; }
        public static ObservableCollection<Tenant> Tenants { get; set; }

        public static Uri GetOdataUri()
        {
            return new Uri(OData4ServiceUri);
        }
    }
}
