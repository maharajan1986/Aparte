namespace Aparte.Credentials
{
    /// <summary>
    /// ApplicationCredential class
    /// </summary>
    public class ApplicationCredential
    {
        /// <summary>
        /// Name of the application
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Public Key of the application
        /// </summary>
        public string PublicKey { get; set; }
    }

    /// <summary>
    /// UserCredential class
    /// </summary>
    public class UserCredential
    {
        /// <summary>
        /// User ID of the user
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Password of the user
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// User Token
    /// Keeping track of PK System User, given Access Token and Refresh Token
    /// </summary>
    public class UserToken
    {
        /// <summary>
        /// PKSystem User in the database
        /// </summary>
        public long? PKSystemUser { get; set; }
        /// <summary>
        /// Access Token of the user
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Refresh Token of the user
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
