using CryptSharp;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Aparte.Security
{
    /// <summary>
    /// Password Class
    /// Handles clear text password hash and save it to database
    /// Hash uses salt
    /// the original password will be impossible to read, but only can compare
    /// </summary>
    public class Password
    {
        static int iterations = 1000;
        static int keyLength = 64;
        static int saltLength = 16;
        static Encoding encoding = new System.Text.UnicodeEncoding();
        static RNGCryptoServiceProvider randomGenerator = new System.Security.Cryptography.RNGCryptoServiceProvider();

        /// <summary>
        /// Main function to match the password in database and the given password from the user
        /// </summary>
        /// <param name="passwordString"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Match(string passwordString, Password password)
        {
            var pwStored = new Password(password.password, password.Salt);
            var pwTarget = new Password(passwordString, pwStored.Salt);
            return pwStored.Compare(pwTarget);
        }

        private static byte[] GenerateSalt()
        {
            var salt = new byte[saltLength];
            randomGenerator.GetBytes(salt);
            return salt;
        }

        private static byte[] GetPassword(string password)
        {
            return encoding.GetBytes(password);
        }

        byte[] password = null;
        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Password() { }
        /// <summary>
        /// Create Password Object for Registration
        /// </summary>
        /// <param name="passwordString"></param>
        public Password(string passwordString) : this(GetPassword(passwordString), GenerateSalt()) { }


        /// <summary>
        /// Create Password Object from User Authentication
        /// </summary>
        /// <param name="passwordString"></param>
        /// <param name="salt"></param>
        public Password(string passwordString, byte[] salt) : this(GetPassword(passwordString), salt) { }


        /// <summary>
        /// Create Password Object from database
        /// </summary>
        /// <param name="passwordBytes"></param>
        /// <param name="saltBytes"></param>
        public Password(byte[] passwordBytes, byte[] saltBytes)
        {
            this.password = passwordBytes;
            this.Salt = saltBytes;
            this.DerivedKey = Pbkdf2.ComputeDerivedKey(new HMACSHA256(password), Salt, iterations, keyLength);
        }

        /// <summary>
        /// DerivedKey in bytes from string password
        /// </summary>
        public byte[] DerivedKey { get; set; }

        /// <summary>
        /// Salt of byte array used
        /// </summary>
        public byte[] Salt { get; set; }

        /// <summary>
        /// Main function to compare 2 hashed password
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool Compare(Password target)
        {
            return (this.Salt.SequenceEqual(target.Salt)) && (this.DerivedKey.SequenceEqual(target.DerivedKey));
        }
    }
}
