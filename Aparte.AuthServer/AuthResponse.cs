using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Aparte.AuthServer
{
    /// <summary>
    /// Http Header Scheme type
    /// Basic Token
    /// Diget Token
    /// Bearer Token
    /// </summary>
    public enum HttpHeaderScheme
    {
        /// <summary>
        /// Basic scheme: Encoded but not encrypted
        /// GET / HTTP/1.1         
        /// Host: example.org 
        /// Authorization: Basic Zm9vOmJhcg==
        /// </summary>
        Basic,
        /// <summary>
        /// Diget scheme
        /// Credentials are hashed and assigned to response (Hash uses MD5)
        /// WWW-Authenticate: Digest realm="Videos", qop="auth,auth-int", nonce="jd839ud9832duj329u9u8ru32rr8u293ur9u329ru"
        /// </summary>
        Digest,
        /// <summary>
        /// Bearer scheme
        /// </summary>
        Bearer
    }
    /// <summary>
    /// Authorization Process Response Class
    /// </summary>
    public static class AuthResponse
    {
        /// <summary>
        /// Routing Mismatch Exception
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpResponseMessage RouteMismatch(HttpRequestMessage request)
        {
            var status = HttpStatusCode.NotFound;
            var response = request.CreateResponse(status);
            var url = response.RequestMessage.RequestUri.AbsoluteUri;
            response.ReasonPhrase = $"{url}: No such url exists";
            response.Content = CreateHtmlConntents($"{url}: No such url exists. Please make sure the url address is correct", status);
            return response;
        }
        /// <summary>
        /// Request does not have HttpHeader
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpResponseMessage NoHeader(HttpRequestMessage request)
        {
            var status = HttpStatusCode.Forbidden;
            var response = request.CreateResponse(status);
            response.ReasonPhrase = "No header information available";
            response.Content = CreateHtmlConntents("No header information available", status);
            return response;
        }
        /// <summary>
        /// Request HttpHeader does not contain Authorization Header element
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpResponseMessage NoAuthorizationHeader(HttpRequestMessage request)
        {
            var status = HttpStatusCode.Unauthorized;
            var response = request.CreateResponse(status);
            response.ReasonPhrase = "No Authorization header available";
            response.Content = CreateHtmlConntents("<b>No Authorization header available</b>", status);
            return response;
        }
        /// <summary>
        /// Request has wrong authorization scheme (should be either Basic, Digest or Bearer)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpResponseMessage WrongAuthorizationScheme(HttpRequestMessage request)
        {
            var status = HttpStatusCode.BadRequest;
            var response = request.CreateResponse(status);
            response.ReasonPhrase = "Authorization Scheme is not correct. It should be either Basic, Digest or Bearer";
            response.Content = CreateHtmlConntents("Authorization Scheme is not correct. It should be either Basic, Digest or Bearer", status);
            return response;
        }
        /// <summary>
        /// Basic Scheme Authorization Header should contain json object
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpResponseMessage BasicSchemeError(HttpRequestMessage request)
        {
            var status = HttpStatusCode.BadRequest;
            var response = request.CreateResponse(status);
            response.ReasonPhrase = "Header should contain json object";

            response.Content = CreateHtmlConntents("Header should contain json object", status, $"{request.RequestUri.Authority}/Views/Tutorials.html");
            return response;
        }
        /// <summary>
        /// Digest Scheme Authorization Header should contain json object
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpResponseMessage DigestSchemeError(HttpRequestMessage request)
        {
            var status = HttpStatusCode.BadRequest;
            var response = request.CreateResponse(status);
            response.ReasonPhrase = "Header should contain json object";
            response.Content = CreateHtmlConntents("Header should contain json object", status);
            return response;
        }
        /// <summary>
        /// Client/Server shared secret key does not match
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpResponseMessage SecretKeyMismatchError(HttpRequestMessage request)
        {
            var status = HttpStatusCode.BadRequest;
            var response = request.CreateResponse(status);
            response.ReasonPhrase = "Client Secrete Key Mismatch";
            response.Content = CreateHtmlConntents("Client Secrete Key Mismatch", status);
            return response;
        }

        private static StringContent CreateHtmlConntents(string message, HttpStatusCode status)
        {
            return CreateHtmlConntents(message, status, null);
        }

        private static StringContent CreateHtmlConntents(string message, HttpStatusCode status, string refLink)
        {
            var info = "<p><b>Aparte - A multi tenancy application</b></p>";
            info += "<p>EPC Integrated Data Hub</p>";
            info += $"<p>Http Status Code: {status.ToString()}</p>";
            if (refLink != null)
            {
                var link = $"<a href=http://{refLink}>Please refer to this help page</a>";
                message = $"{message} - {link}";
            }
            var contents = $"<html><body>{info}<p>{message}</p><body></html>";
            var stringContent = new StringContent(message);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return stringContent;
        }
        /// <summary>
        /// Method to create error response message
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, string message, HttpRequestMessage request)
        {
            message = message.Replace(Environment.NewLine, String.Empty);
            var response = request.CreateResponse(statusCode);
            response.ReasonPhrase = message;
            response.Content = CreateTextConntents(message, statusCode);
            return response;
        }

        private static StringContent CreateTextConntents(string message, HttpStatusCode status)
        {
            var contents = string.Format("Http Status Code: {0}{1}{2}", status, Environment.NewLine, message);
            var stringContent = new StringContent(contents);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return stringContent;
        }


        #region Internal Method
        internal static bool IsBasicScheme(string scheme)
        {
            return scheme.ToUpper() == HttpHeaderScheme.Basic.ToString().ToUpper();
        }

        internal static bool IsDigestScheme(string scheme)
        {
            return scheme.ToUpper() == HttpHeaderScheme.Digest.ToString().ToUpper();
        }

        internal static bool IsBearerScheme(string scheme)
        {
            return scheme.ToUpper() == HttpHeaderScheme.Bearer.ToString().ToUpper();
        }

        internal static bool IsAuthorizationScheme(HttpRequestMessage request)
        {
            var scheme = request.Headers.Authorization.Scheme;
            return (IsBasicScheme(scheme) || IsDigestScheme(scheme) || IsBearerScheme(scheme));
        }
        #endregion

    }
}
