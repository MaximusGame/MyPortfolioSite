using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace MyPortfolioSite.Models
{
    public class RequestCrypto
    {
        private static string _response = "";


        public static string GetRequestCrypto (string CryptoName)
        {
            string url = "https://min-api.cryptocompare.com/data/pricemulti?fsyms=" + CryptoName + "&tsyms=USD";

            using (var webClient = new WebClient())
            {
                _response = webClient.DownloadString(url);
            }
            var customer = JsonConvert.DeserializeObject<JObject>(_response).First.First;

            if (customer.HasValues == true)
            {
                string _a;
                _a = CryptoName + " = " + customer["USD"].ToString() + " $";
                return _a;
            }
            else
            {
                return "It is not correct cryptocurrency name";
            }
            
        }

       
    }
}