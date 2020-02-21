using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MyPortfolioSite.Folder_ICUTech_test
{
    public class ResponseBedObject
    {
        public int ResultCode { get; set; }
        public string ResultMessage { get; set; }
    }

    public class ResponseGoodObject
    {
        public int EntityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int EmailConfirm { get; set; }
        public int MobileConfirm { get; set; }
        public int CountryID { get; set; }
        public int Status { get; set; }
        public string lid { get; set; }
        public string FTPHost { get; set; }
        public int FTPPort { get; set; }
    }


    public class User
    {
        public string Name { get; set; } 
        public string Password { get; set; }
    }

    public class Request_ICUTech
    {
        private static string _url = "http://isapi.icu-tech.com/ICUTech-test.dll/soap/IICUTech";
        public static string User_Name { get; set; }
        public static string User_Password { get; set; }
        private static string _IP = "";

        public static object CallWebService()
        {            
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            HttpWebRequest webRequest = CreateWebRequest(_url, null);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            asyncResult.AsyncWaitHandle.WaitOne();

            string soapResult;
            string Response = "";
            string color = "";
            ResponseGoodObject EntityInfo = null;
            object json = null;

            XmlDocument doc = new XmlDocument();

            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                    doc.LoadXml(soapResult);
                }

            }

            foreach (XmlNode item in doc.GetElementsByTagName("return"))
            {
                Response = item.FirstChild.Value;
            }

            try
            {
                ResponseBedObject obj_bed = JsonConvert.DeserializeObject<ResponseBedObject>(Response);

                if (obj_bed.ResultMessage != "" && obj_bed.ResultMessage == "User not found")
                {
                    color = "RED";
                    json = new {color, EntityInfo };
                    return json;
                }
                else
                {
                   ResponseGoodObject obj_good = JsonConvert.DeserializeObject<ResponseGoodObject>(Response);
                   if (obj_good.EntityId > 0)
                   {
                        EntityInfo = new ResponseGoodObject
                        {
                            EntityId = obj_good.EntityId,
                            FirstName = obj_good.FirstName,
                            LastName = obj_good.LastName,
                            Company = obj_good.Company,
                            Address = obj_good.Address,
                            City = obj_good.City,
                            Country = obj_good.Country,
                            Zip = obj_good.Zip,
                            Phone = obj_good.Phone,
                            Mobile = obj_good.Mobile,
                            Email = obj_good.Email,
                            EmailConfirm = obj_good.EmailConfirm,
                            MobileConfirm = obj_good.MobileConfirm,
                            CountryID = obj_good.CountryID,
                            Status = obj_good.Status,
                            lid = obj_good.lid,
                            FTPHost = obj_good.FTPHost,
                            FTPPort = obj_good.FTPPort
                        };

                        color = "GREEN";
                        json = new { color, EntityInfo };
                        return json;
                   }
                }
            }
            catch
            {
                color = "RED";
                json = new { color, EntityInfo };
                return json;
            }

            color = "RED";
            json = new { color, EntityInfo };
            return json;

        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope()
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(@"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
            <Body>
              <Login xmlns = ""http://tempuri.org/"">
              <UserName>" + User_Name + "</UserName>" +
              "<Password>" + User_Password + "</Password>" +
              "<IPs>" + _IP + "</IPs></Login>" +
              "</Body></Envelope>");
            return soapEnvelopeDocument;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

    }


}