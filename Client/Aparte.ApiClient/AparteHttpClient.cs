using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Aparte.ApiClient
{
    public class AparteHttpClient : HttpClient
    {
        public static class ContentType
        {
            public const string APP_FORM_URLENCODED = "application/x-www-form-urlencoded";
            public const string APP_JSON = "application/json";
            //application/x-javascript
            //text/javascript
            //text/x-javascript
            //text/x-json
        }


        public AparteHttpClient(string baseAddress) : this(baseAddress, ContentType.APP_JSON)
        {

        }

        public AparteHttpClient(string baseAddress, string contentType) : this(baseAddress, contentType, null)
        {
        }

        public AparteHttpClient(string baseAddress, AuthenticationHeaderValue header) : this(baseAddress, ContentType.APP_JSON, header)
        {
        }
        public AparteHttpClient(string baseAddress, string contentType, AuthenticationHeaderValue header): base(GetHttpClientHandler())
        {
            base.BaseAddress = new Uri(baseAddress);
            base.DefaultRequestHeaders.Accept.Clear();
            base.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            base.DefaultRequestHeaders.Authorization = header;
        }
        private static HttpClientHandler GetHttpClientHandler()
        {
            // TODO : Handle Proxy settings here when corporate network.
            //var clientProxy = WinClient.Proxy;
            //if (clientProxy != null && clientProxy.UseProxy)
            //{
            //    string proxyUri = clientProxy.ProxyServer;
            //    string httpUserName = clientProxy.UserId, httpPassword = clientProxy.Password;
            //    NetworkCredential proxyCreds = new NetworkCredential(httpUserName, httpPassword);
            //    WebProxy proxy = new WebProxy(proxyUri, false)
            //    {
            //        UseDefaultCredentials = false,
            //        Credentials = proxyCreds,
            //        Address = new Uri(clientProxy.ServerAddress),
            //        BypassProxyOnLocal = clientProxy.BypassProxyOnLocal
            //    };
            //    HttpClientHandler httpClientHandler = new HttpClientHandler()
            //    {
            //        Proxy = proxy,
            //        PreAuthenticate = true,
            //        UseDefaultCredentials = false,
            //        Credentials = proxyCreds
            //    };
            //    return httpClientHandler;
            //}
            return new HttpClientHandler();
        }

    }
}
