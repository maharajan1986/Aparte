using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.AuthServer
{
    /// <summary>
    /// Hardcoded AuthenticationServer Public Key/Private Key pair
    /// </summary>
    public static class AuthenticationServer
    {
        /// <summary>
        /// Constant Value of Public Key
        /// </summary>
        public const string PUBLIC_KEY = "<RSAKeyValue><Modulus>3UiYG+d+lxxBXaBeIyF8kuFqIY3NSNAdFq4PW7EYep3Zd3NsdG24WemSbnTzQpIPc+Mn/GqFI0APeJ0BQ8Lo6rqDUQAWEPshFWDundF0TNC7Bh5AjNLI4cUQBFzLRgAh/YLJnVWT1W3ciy88ECgLF0WUtCfFmBy5adh8rgB5obU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        /// <summary>
        /// Constant Value of Private Key - this should be absolutely secret
        /// </summary>
        public const string PRIVATE_KEY = "<RSAKeyValue><Modulus>3UiYG+d+lxxBXaBeIyF8kuFqIY3NSNAdFq4PW7EYep3Zd3NsdG24WemSbnTzQpIPc+Mn/GqFI0APeJ0BQ8Lo6rqDUQAWEPshFWDundF0TNC7Bh5AjNLI4cUQBFzLRgAh/YLJnVWT1W3ciy88ECgLF0WUtCfFmBy5adh8rgB5obU=</Modulus><Exponent>AQAB</Exponent><P>8+oS223yfYx2igDYcRC5z83b5wLkuu3gi6/Ci3u8br2EBuvSFsgMpeT51Dm8Lf9qbuLbCZOiUE175R4BS43bQQ==</P><Q>6D91/FbsdMyACs3yI30/l34Axmdi8WGi14Ni3UqhJia76uc+iJnbe3qL9oIN0dkR4ktSv/KJv0/kpVZDpJotdQ==</Q><DP>oTdksV5RecQ+kWaPqOPKPNyu7VjPP/J8iTdpmfH2ESf4PO7flKkzGu9mZWynwatheNs+tWy8SuF782tKpdqkQQ==</DP><DQ>PYFw8Z7jiBsQXcwksBlfWfNYqTKAFYTR51k4OXqmKsBfS9ppyStV4OGXZ3URy908yz0/cO3+ZNf3qYGq8FCUcQ==</DQ><InverseQ>h7xIbtx5GFFakDWU9pWcVHnueIMq5UiipRWw2PUdueQJR33xSuZpIkBlIZvR6lH4VKTMRyvoW7sWgOfL3lSzGA==</InverseQ><D>Z1xFUKNtCgB0t9r0ncxCeAk6nbmyrdQoAjQDkHzERmH89kK/4hJuDfGAKIAQMIQxG5x7TPgkgDaoA1qzAIfGax+RKGFMq+ZWM6x9IRpNK6EP/3dpXpt+DYwAs1OgB0Awcp+HympAH+ueaiprkFmuaZrHWPQlA7+L3PzKCOlKdAE=</D></RSAKeyValue>";

    }
}
