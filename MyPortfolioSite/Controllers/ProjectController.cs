using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;
using MyPortfolioSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyPortfolioSite.Controllers
{
    public class ProjectController : Controller
    {
        private DB_Contex db = new DB_Contex();

        // GET: Project
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Project/Details/{id}
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IProjectRepository repo = new ProjectManager(db);
            Project project = await repo.GetProject(id);

            if (project == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string[] projacts_people = project.Peoples.Select(x => x.Name).ToArray();

            object jsonObject = new { project.Name, project.Price, projacts_people };

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }

        //POST: Project/Create
        [HttpPost]
        public async Task<HttpResponseMessage> Create([Bind(Include = "Name, Price")] Project project)
        {
            if (project.Name != null && project.Price != null)
            {
                IProjectRepository repo = new ProjectManager(db);
                int CreatePeopleId = await repo.CreateProject(project);
                HttpResponseMessage responseMessage = new HttpResponseMessage() { StatusCode = HttpStatusCode.Created };
                responseMessage.Headers.Add("People_ID", CreatePeopleId.ToString());
                return responseMessage;
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        // POST: Project/Edit/{id}
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Price")] Project project, int id)
        {
            if (project.Name != null && project.Price != null)
            {
                IProjectRepository repo = new ProjectManager(db);
                bool e = await repo.EditProject(project, id);

                if (e == false)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // GET: Project/Delete/{id}
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IProjectRepository repo = new ProjectManager(db);
            bool e = await repo.DeleteProject(id);

            if (e == false)
            {
                return HttpNotFound();
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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