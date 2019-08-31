using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

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
    }
}