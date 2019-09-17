using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;
using MyPortfolioSite.FolderForCryptoTest;
using MyPortfolioSite.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using static MyPortfolioSite.Models.DB_Manager;

namespace MyPortfolioSite.Controllers
{
 
    public class HomeController : Controller
    {
        private DB_Manager DB_Manager = new DB_Manager();
        private CryptoModel CryptoModel = new CryptoModel();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Key information";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "We can contact me:";

            return View();
        }

        public ActionResult PostGetRequest()
        {
            return View();
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

        public ActionResult EntityFramework_and_DB()
        {
            var Model = new Context_Model()
            {
                Peoples_Model = DB_Manager.ReturneDB_People(),
                Projects_Model = DB_Manager.ReturnDB_Projects()
            };

            return View(Model);
        }

    
        public ActionResult CreatePeople()
        {
            ViewBag.Message = "CreatePeople";
            return View();
        }

        [HttpPost]
        public ActionResult CreateNewPeople([Bind(Include = "Id,Name,Phone")] People people)
        {
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


        public ActionResult ICUTech_Test()
        {
            return View();
        }

        public ActionResult Crypto(string CryptoName)
        {
            if (CryptoName == null)
            {
                return View();
            }
            else
            {
                var SeachCrypto = CryptoModel.Cryptos.Where(a => a.Name == CryptoName);
                SelectionCryptoCurrency selectionCryptoCurrency = new SelectionCryptoCurrency();

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
            var SeachCrypto = CryptoModel.Cryptos.Where(a => a.Name == CryptoName);
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
    }
}