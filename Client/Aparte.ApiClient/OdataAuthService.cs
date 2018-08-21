using Aparete.JWT;
using Aparte.ApiClient.ODataService;
using Microsoft.OData.Client;
using Microsoft.OData.Core;
using System;
using System.Collections.Generic;

namespace Aparte.ApiClient
{
    public class ODataRequestMessage : HttpWebRequestMessage
    {
        public delegate void WebHttpException(Exception exception);
        public event WebHttpException OnHttpWebException;

        public string Response { get; set; }
        public ODataRequestMessage(DataServiceClientRequestMessageArgs args) : base(args) {}

        public override IODataResponseMessage GetResponse()
        {
            try
            {
                return base.GetResponse();
            }
            catch (Exception e)
            {
                OnHttpWebException?.Invoke(e);
            }
            return null;
        }
    }
    public class OdataAuthService : Container
    {
        public event ODataRequestMessage.WebHttpException OnHttpWebException;
        public OdataAuthService() : this(null) { }
        public OdataAuthService(Dictionary<string, string> queryParameters) : base(WinClient.GetOdataUri())
        {
            if (WinClient.IsTokenExpired)
            {
                Services.AuthorizeClientService.RefreshToken(WinClient.PKSystemUser, WinClient.AccessToken, WinClient.RefreshToken).Wait();
            }
            this.MergeOption = Microsoft.OData.Client.MergeOption.PreserveChanges;
            var headerString = HttpHeader.ODataVerificationHeader(WinClient.ServerPublicKey, WinClient.PKSystemUser, WinClient.FKTenant, queryParameters);

            ODataRequestMessage message = null;
            
            base.Configurations.RequestPipeline.OnMessageCreating = (args) =>
            {
                message = new ODataRequestMessage(args);                                
                message.OnHttpWebException += Message_OnWebException;
                return message;
            };                        
            this.SendingRequest2 += (sender, args) =>
            {
                //var proxy = WinClient.Proxy;
                //if (proxy != null && proxy.UseProxy)
                //{
                //    if (!args.IsBatchPart)
                //    {
                //        var request = ((HttpWebRequestMessage)args.RequestMessage).HttpWebRequest;
                //        request.Proxy = proxy.GetWebProxy();
                //    }
                //}                                
                args.RequestMessage.SetHeader("Authorization", HttpHeaderScheme.Bearer.ToString() + " " + headerString);
            };
            this.ReceivingResponse += (sender, args) =>
            {
                var descriptor = args.Descriptor;
                var msg = args.ResponseMessage;
            };
        }

        private void Message_OnWebException(Exception exception)
        {
            OnHttpWebException?.Invoke(exception);
        }
    }
}
