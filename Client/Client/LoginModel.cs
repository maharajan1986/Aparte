using Aparte.ApiClient;
using Aparte.ApiClient.Aparte.Models;
using Aparte.ApiClient.Services;
using Aparte.MasterData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class LoginModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public LoginModel(string userId,string password)
        {
            this.UserId = userId;
            this.Password = password;
        }
        /// <summary>
        /// Comments by MH
        /// Actual command to run when a user press log in
        /// 1. Exchange Public Key with Api Server
        /// 2. Verify this client application
        /// 3. Verify this user
        /// 4. If success, proceed to get the list of tenants to be displayed in the combo box
        /// TODO: This log in method should only deal with user authentication, not Client authentication, so ExchangePublicKey() and VerifyClient() should be only called once before the application starts, but cannot find appropriate timing.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> Login(CancellationToken token)
        {
            var isExhangeSuccess = await AuthorizeClientService.ExchangePublicKey();
            var isAuthorizeSuccess = await AuthorizeClientService.VerifyClient();
            WinClient.IsUserAuthenticated = await AuthorizeClientService.GetAccessToken(this.UserId, this.Password, token);

            if (WinClient.IsUserAuthenticated)
            {
                var odata = new OdataAuthService();
                WinClient.Tenants = new ObservableCollection<Tenant>(odata.Tenants);
                try
                {
                    //var attributes = odata.GetAttributes(ClientRule.IsUniqe.ToServerEnum()).ToList();
                    for (int i = 0; i < 1100; i++)
                    {
                        odata.AddToTenants(new Tenant() { PK = 200 + i, Name = $"Test{i}", Code = $"Test{i}" });
                    }                    
                    odata.SaveChanges(Microsoft.OData.Client.SaveChangesOptions.BatchWithSingleChangeset);
                }
                catch (Exception e)
                {
                    throw e;
                }              


                WinClient.SystemUserId = this.UserId;                
            }
            else
            {
                Console.WriteLine("The User name or Password provided is incorrect.Please correct the errors and try again.", "Connect to JGC Integrated Data Hub");
            }
            return false;
        }
    }

    public static class EnumHelper
    {
        public static Aparte.ApiClient.Aparte.MasterData.AttributeRule ToServerEnum(this Enum srcEnum)
        {            
            return (Aparte.ApiClient.Aparte.MasterData.AttributeRule)
                Enum.Parse(typeof(Aparte.ApiClient.Aparte.MasterData.AttributeRule), srcEnum.ToString(), true);
        }
    }

    public enum ClientRule : long
    {
        None = 0,        
        IsUniqe = 1,        
        UsePreset = 2
    }
}
