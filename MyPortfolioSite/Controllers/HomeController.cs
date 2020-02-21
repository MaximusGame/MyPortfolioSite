using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;
using MyPortfolioSite.Folder_ICUTech_test;
using MyPortfolioSite.FolderForCryptoTest;
using MyPortfolioSite.FolderForScenriosDownloaderTest;
using MyPortfolioSite.Models;
using Newtonsoft.Json;
using System;
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

namespace MyPortfolioSite.Controllers
{

    public class HomeController : Controller
    {
        private DB_Contex db = new DB_Contex();
        public static int ProjectID { get; set; }
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

        /// <summary>
        /// Data Base View and requests
        /// </summary>
        /// <returns></returns>
        public ActionResult DataBase()
        {
            var Model = new Context_Model()
            {
                Peoples_Model = db.Peoples.ToList(),
                Projects_Model = db.Projects.ToList()
            };

            return View(Model);
        }

        public ActionResult TableData()
        {
            var Model = new Context_Model()
            {
                Peoples_Model = db.Peoples.ToList(),
                Projects_Model = db.Projects.ToList()
            };
            return PartialView("DataPersonTable", Model);
        }

        //public ActionResult CreatePeople()
        //{
        //    ViewBag.Message = "CreatePeople";
        //    return View();
        //}

        //[HttpPost]
        //public async Task<ActionResult> CreateNewPeople([Bind(Include = "Id,Name,Phone")] People people, HttpPostedFileBase upload)
        //{
        //    if (people != null)
        //    {
        //        if (upload != null)
        //        {
        //            string fileName = people.Id.ToString() + "_" + Path.GetFileName(upload.FileName);
        //            string ext = Path.GetExtension(upload.FileName);

        //            if ( ext.ToLower() == ".png")
        //            {
        //                upload.SaveAs(Server.MapPath(Path.Combine("~/Fotos/", fileName)));
        //                people.FotoName = fileName;
        //            }
        //        }

        //        IPeopleRepository repo = new PeopleManager(db);
        //        await repo.CreatePeople(people);
        //    }

        //    return RedirectToAction("DataBase");
        //}


        public ActionResult EditPeople(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            People people = db.Peoples.Find(id);

            if (people == null)
            {
                return HttpNotFound();
            }
            return View(people);
        }

        [HttpPost]
        public async Task<ActionResult> EditPeople([Bind(Include = "Id,Name,Phone")] People people)
        {
            if (ModelState.IsValid)
            {
                IPeopleRepository repo = new PeopleManager(db);
                bool e = await repo.EditPeople(people, people.Id);
                if (e == false)
                {
                    return View(people);
                }
                return RedirectToAction("DataBase");
            }
            return View(people);
        }

        //public ActionResult DeletePeople(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    People people = db.Peoples.Find(id);

        //    if (people == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(people);
        //}

        //[HttpPost]
        //public async Task<ActionResult> DeletePeople(int id)
        //{
        //    IPeopleRepository repo = new PeopleManager(db);
        //    bool e = await repo.DeletePeople(id);

        //    if (e == false)
        //    {
        //        return HttpNotFound();
        //    }

        //    return RedirectToAction("DataBase");
        //}


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
                Project project = db.Projects.Find(ProjectID);
                People people = db.Peoples.Find(People_ID);
                people.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("DataBase");
            }
            else
                return RedirectToAction("DataBase");
        }

        [HttpPost]
        public void AddProjectId(string id)
        {
            ProjectID = int.Parse(id);
        }
               
            
        public ActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewProject([Bind(Include = "Id,Name,Price")] Project project)
        {
            IProjectRepository repo = new ProjectManager(db);
            int CreatePeopleId = await repo.CreateProject(project);
            return RedirectToAction("DataBase");
        }

        public ActionResult EditProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = db.Projects.Find(id); ;

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [HttpPost]
        public async Task<ActionResult> EditProject([Bind(Include = "Id,Name,Price")] Project project, int id)
        {
            if (ModelState.IsValid)
            {
                IProjectRepository repo = new ProjectManager(db);
                await repo.EditProject(project, id);
                return RedirectToAction("DataBase");
            }
            return View(project);
        }

        //public ActionResult DeleteProject(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Project project = null; // DB_Manager.ReturnProject(id);
        //    //IProjectRepository repo = new ProjectManager(db);
        //    //bool e = await repo.DeleteProject(id);

        //    if (project == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(project);
        //}

        //[HttpPost]
        //public ActionResult DeleteProject(int id)
        //{
        //    //DB_Manager.DeleteProject(id);
        //    return RedirectToAction("DataBase");
        //}

        public ActionResult AddProjectToPeople(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ModelForAddProjectToPeople modelForAddProject = new ModelForAddProjectToPeople
            {
                People = db.Peoples.Find(id), // DB_Manager.ReturnPeople(id),
                Projects_Model = db.Projects.ToList(), // DB_Manager.ReturnDB_Projects()
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
                string[] a = {"","","" }; // DB_Manager.Imagas();
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

        public ActionResult ICUTech_test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ICUTech_Login([Bind(Include = "Name,Password")] User user)
        {
            if (user != null)
            {
               Request_ICUTech.User_Name = user.Name;
               Request_ICUTech.User_Password = user.Password;
            }
            
            return Json( Request_ICUTech.CallWebService(), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}