using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MyPortfolioSite.FolderForCryptoTest
{
    public class Chart
    {
        public string Date { get; set; }
        public int Price { get; set; }
    }

    public class RootObject
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string CreatedOn { get; set; }
        public List<Chart> Chart { get; set; }
    }

    public class SelectionCryptoCurrency
    {
        public string Name { get; set; }
        public List<RootObject> CryptoCurrency;
    }

    public class CryptoModel
    {
        //public static List<RootObject> Cryptos;
        //public static RootObject RootObject;
        private static string json;

        //public CryptoModel()
        //{
        //    if ((Cryptos == null) || (Cryptos.Count == 0))
        //    {

        //        Cryptos = new List<RootObject>();

        //        try
        //        {
        //            using (StreamReader r = new StreamReader(HostingEnvironment.MapPath("/FolderForCryptoTest/Crypto.json")))
        //            {
        //                json = r.ReadToEnd();
        //            }

        //            var obj = JsonConvert.DeserializeObject<List<RootObject>>(json);

        //           foreach (var i in obj)
        //           {
        //              List<Chart> mychart = i.Chart;
        //              Cryptos.Add(new RootObject { Id = i.Id, Ticker = i.Ticker, Name = i.Name, CreatedOn = i.CreatedOn, Chart = mychart });
        //           }
        //        }
        //        catch (Exception e)
        //        {
        //            string r = e.ToString();
        //        }                              
        //    }
        //}

        public static void FillList (List<RootObject> CryptosList)
        {            
            CryptosList.Clear();

            try
            {
                using (StreamReader r = new StreamReader(HostingEnvironment.MapPath("/FolderForCryptoTest/Crypto.json")))
                {
                    json = r.ReadToEnd();
                }

                var obj = JsonConvert.DeserializeObject<List<RootObject>>(json);

                foreach (var i in obj)
                {
                    List<Chart> mychart = i.Chart;
                    CryptosList.Add(new RootObject { Id = i.Id, Ticker = i.Ticker, Name = i.Name, CreatedOn = i.CreatedOn, Chart = mychart });
                }
            }
            catch (Exception e)
            {
                string r = e.ToString();
            }
        }
    }
}