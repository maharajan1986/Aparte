using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Aparete.JWT
{
    /// <summary>
    /// Stataic Base64UrlEncoder class
    /// Conversion from byte[] to Base64UrlString
    /// Conversion from Base64UrlString to byte[]
    /// Base 64 Design: 64 characters safe to use, the characters should be members of a subset common to most encodings, and also printable
    /// A–Z, a–z, and 0–9, + and /
    /// </summary>
    public static class Base64UrlEncoder
    {
        /// <summary>
        /// Check if the string is Base64 string
        /// 1. the string should be multiple of 4
        /// 2. Regular expression ^[a-zA-Z0-9\+/]*={0,3}$ is used for checking
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsBase64String(this string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }
        /// <summary>
        /// Convert byte array to Base64UrlString
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64UrlString(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        /// <summary>
        /// Convert Base64UrlString to byte array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] FromBase64Url(this string input)
        {
            input = input
                .Replace('-', '+')
                .Replace('_', '/');
            int pad = 4 - (input.Length % 4);
            pad = pad > 2 ? 0 : pad;

            input = input.PadRight(input.Length + pad, '=');
            return Convert.FromBase64String(input);
        }

        /// <summary>
        /// Convert from normal string to Base64UrlString
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetBase64UrlFromNormalString(this string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return bytes.ToBase64UrlString();
        }

        /// <summary>
        /// Convert from Base64String to normal string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetNormalStringFromBase64Url(this string input)
        {
            var bytes = input.FromBase64Url();
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
