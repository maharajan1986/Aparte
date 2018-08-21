using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aparte.ApiClient;
using Client;

namespace Aparte.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new LoginModel("sakurai.hironori@jgc.com", "sakurai");
            var token = new System.Threading.CancellationToken();
            var r = model.Login(token).Result;

            var tenants = WinClient.Tenants;       
        }

    }
}
