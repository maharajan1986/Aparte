using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Aparete.Json
{
    /// <summary>
    /// JsonSerializer class
    /// Handles serialize object to json or deserialize json to object
    /// </summary>
    public static class JsonSerializer
    {
        /// <summary>
        /// Serialize content object to json string
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Serialize(object content)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(content);
        }

        /// <summary>
        /// Deserialize json string to object of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string jsonString)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// Serialize content to string and return http response type of string as application/json
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static StringContent GetStringContent(object content)
        {
            var stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(content);
            return new StringContent(stringContent, Encoding.UTF8, "application/json");
        }
    }
}
