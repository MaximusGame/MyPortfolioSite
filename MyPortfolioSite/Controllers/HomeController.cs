using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;
using MyPortfolioSite.FolderForCryptoTest;
using MyPortfolioSite.FolderForScenriosDownloaderTest;
using MyPortfolioSite.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using static MyPortfolioSite.Models.DB_Manager;

namespace MyPortfolioSite.Controllers
{
 
    public class HomeController : Controller
    {
        
       // private static DB_Contex db = new DB_Contex();

        private DB_Manager DB_Manager = new DB_Manager(null);

        private List<RootObject> CryptosList = new List<RootObject>();

        public ActionResult Index()
        {
            return View();
        }

        public string SendMail(string user_name, string user_email, string text_comment)
        {
            try
            {
                MailMessage objMailMessage = new MailMessage("user@mail.com", "maximuskavun@gmail.com")
                {              
                   Subject = "Message from my Site",
                   IsBodyHtml = true,
                   Body = text_comment + " (From: " + user_name + "  E-Mail: " + user_email + ")"
                };
                SmtpClient objsmtp = new SmtpClient
                {
                   EnableSsl = true // this statement is required in case you are planning to use Gmail account/Google apps account
                };
                objsmtp.Send(objMailMessage);
                return "Your message send me successful.";
            }
            catch
            {
                return "";
            }
            
        }

        public ActionResult REST_API()
        {
            return View();
        }

        public ActionResult DataBase()
        {
            var Model = new Context_Model()
            {
                Peoples_Model = DB_Manager.ReturneDB_People(),
                Projects_Model = DB_Manager.ReturnDB_Projects()
            };

            return View(Model);
        }



        public FileResult GetFile()
        {
            string file_path = Server.MapPath("~/MyFiles/MyResume.pdf");
            string file_type = "application/pdf";
            string file_name = "Resume.pdf";
            return File(file_path, file_type, file_name);
        }

        public FileResult GetImage(string ImegeName)
        {
            if (System.IO.File.Exists(Server.MapPath("~/Fotos/" + ImegeName)))
            {
                string file_path = Server.MapPath("~/Fotos/" + ImegeName);
                string file_type = "image/png";
                string file_name = ImegeName;
                return File(file_path, file_type, file_name);
            }
            else
            {
                string file_path = Server.MapPath("~/Fotos/avatar.png");
                string file_type = "image/png";
                string file_name = "avatar.png";
                return File(file_path, file_type, file_name);
            }
           
        }       

        public string GetPriceCrypto (string CryptoName)
        {
            return RequestCrypto.GetRequestCrypto(CryptoName);
            
        }

        [HttpPost]
        public ActionResult AddPeopleInProject(int? People_ID)
        {
            if (ProjectID != 0)
            {
                DB_Manager.PeopleInProject(People_ID);
                return RedirectToAction("EntityFramework_and_DB");
            }
            else
                return RedirectToAction("EntityFramework_and_DB");
        }

        [HttpPost]
        public void AddProjectId(string id)
        {
            ProjectID = ConvertProjectID(id);
        }

        


        public ActionResult CreatePeople()
        {
            ViewBag.Message = "CreatePeople";
            return View();
        }

        [HttpPost]
        public ActionResult CreateNewPeople([Bind(Include = "Id,Name,Phone")] People people, HttpPostedFileBase upload)
        {

            if (upload != null)
            {
                string fileName = Path.GetFileName(upload.FileName);
                string ext = Path.GetExtension(upload.FileName);

                if (/*ext.ToLower() != ".jpg" &&*/ ext.ToLower() != ".png" /*&& ext.ToLower() != ".jpeg"*/)
                {
                    people.FotoName = "avatar.png";
                }
                else
                {
                    upload.SaveAs(Server.MapPath(Path.Combine("~/Fotos/", fileName)));
                    people.FotoName = fileName;
                }
                // сохраняем файл в папку Files в проекте
                //string FotoFolderPath = Server.MapPath("~/Fotos/");
                //upload.SaveAs(Server.MapPath("~/Fotos/" + fileName));

            }
            else
            {
                people.FotoName = "avatar.png";
            }

            DB_Manager.AddNewPeople(people);
            return RedirectToAction("EntityFramework_and_DB");
        }

        public ActionResult EditPeople(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            People people = DB_Manager.ReturnPeople(id);

            if (people == null)
            {
                return HttpNotFound();
            }
            return View(people);
        }

        [HttpPost]
        public ActionResult EditPeople([Bind(Include = "Id,Name,Phone")] People people)
        {
            if (ModelState.IsValid)
            {
                DB_Manager.EditePeople(people);
                return RedirectToAction("EntityFramework_and_DB");
            }
            return View(people);
        }

        public ActionResult DeletePeople(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            People people = DB_Manager.ReturnPeople(id);

            if (people == null)
            {
                return HttpNotFound();
            }
            return View(people);
        }

        [HttpPost]
        public ActionResult DeletePeople(int id)
        {
            string foto = DB_Manager.ReturnFotoFilePeople(id);

            if ("avatar.png" != foto)
            {
                System.IO.File.Delete(Server.MapPath("~/Fotos/" + DB_Manager.ReturnFotoFilePeople(id)));
            }

            DB_Manager.DeletePeople(id);
            return RedirectToAction("EntityFramework_and_DB");
        }


        public ActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNewProject([Bind(Include = "Id,Name,Price")] Project project)
        {
            DB_Manager.AddNewProject(project);
            return RedirectToAction("EntityFramework_and_DB");
        }

        public ActionResult EditProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = DB_Manager.ReturnProject(id);

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [HttpPost]
        public ActionResult EditProject([Bind(Include = "Id,Name,Price")] Project project)
        {
            if (ModelState.IsValid)
            {
                DB_Manager.EditeProject(project);
                return RedirectToAction("EntityFramework_and_DB");
            }
            return View(project);
        }

        public ActionResult DeleteProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = DB_Manager.ReturnProject(id);

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [HttpPost]
        public ActionResult DeleteProject(int id)
        {
            DB_Manager.DeleteProject(id);
            return RedirectToAction("EntityFramework_and_DB");
        }

        public ActionResult AddProjectToPeople(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ModelForAddProjectToPeople modelForAddProject = new ModelForAddProjectToPeople
            {
                People = DB_Manager.ReturnPeople(id),
                Projects_Model = DB_Manager.ReturnDB_Projects()
            };

            if (modelForAddProject.People == null)
            {
                return HttpNotFound();
            }
            return View(modelForAddProject);
        }




        //Scenarios Dowloader
        public ActionResult ScenariosDownload()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetScenariosList(string ScenarioLanguage)
        {
            object jsonObject = null;

            switch (ScenarioLanguage)
            {
                case "Eng":
                    string ScenarioEng = System.IO.File.ReadAllText(Server.MapPath("~/FolderForScenriosDownloaderTest/Scenarios/Eng/ScenarioEng.json"));
                    jsonObject = ScenariosContext.GetJSON(ScenarioEng);
                    break;

                case "Esp":
                    string ScenarioEsp = System.IO.File.ReadAllText(Server.MapPath("~/FolderForScenriosDownloaderTest/Scenarios/Esp/ScenarioEsp.json"));
                    jsonObject = ScenariosContext.GetJSON(ScenarioEsp);
                    break;

                default:
                    jsonObject = new { ScenarioName = "Not Scenario Name", ScenariyDescription = "Not Scenario Description" };
                    break;
            }

            return Json(jsonObject, JsonRequestBehavior.AllowGet);

        }


        // Crypto Currency
        public ActionResult Crypto(string CryptoName)
        {
            CryptoModel.FillList(CryptosList);

            if (CryptoName == null)
            {
                SelectionCryptoCurrency selectionCryptoCurrency = new SelectionCryptoCurrency
                {
                    CryptoCurrency = CryptosList

                };
                return View(selectionCryptoCurrency);
            }
            else
            {
                var SeachCrypto = CryptosList.Where(a => a.Name == CryptoName);

                SelectionCryptoCurrency selectionCryptoCurrency = new SelectionCryptoCurrency
                {
                    CryptoCurrency = CryptosList

                };

                foreach (RootObject i in SeachCrypto)
                {
                    selectionCryptoCurrency.Name = i.Name;
                }
                return View(selectionCryptoCurrency);
                
            }
        }

        [HttpGet]
        public ActionResult GetSelectionCurrency(string CryptoName)
        {
            CryptoModel.FillList(CryptosList);
            var SeachCrypto = CryptosList.Where(a => a.Name == CryptoName);
            RootObject rootObject = new RootObject();

            string[] DateForChart = new string[3];
            string[] PriceForChart = new string[3];

            foreach (RootObject i in SeachCrypto)
            {
                rootObject.Id = i.Id;
                rootObject.Ticker = i.Ticker;
                rootObject.Name = i.Name;
                rootObject.CreatedOn = i.CreatedOn;
                List<Chart> mychart = i.Chart;

                int counter = 0;
                foreach (var a in mychart)
                {
                    DateForChart[counter] += a.Date;
                    PriceForChart[counter] += a.Price;
                    counter++;
                }            
            }
            return Json(new { rootObject.Id, rootObject.Ticker, rootObject.Name, rootObject.CreatedOn, DateForChart, PriceForChart }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetListImageName(string Image)
        {
            if (Image == "all")
            {
                string[] a = DB_Manager.Imagas();
                return Json(a);
            }
            else
            {
                if (Image == "no")
                {
                    return Json("File not found");
                }
                else
                {
                    return RedirectToAction("GetImage", new RouteValueDictionary(new { ImegeName = Image }));
                }
            }


        }

        public ActionResult ErrorMessage(string Mes)
        {
            return Json(Mes);
        }


        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}