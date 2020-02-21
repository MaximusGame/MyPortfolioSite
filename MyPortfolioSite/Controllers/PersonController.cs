using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;
using MyPortfolioSite.Models;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;

namespace MyPortfolioSite.Controllers
{
    public class PersonController : Controller
    {
        private DB_Contex db = new DB_Contex();

        // GET: Person/All
        public ActionResult All()
        {
            People[] people = new People[db.Peoples.Count()];

            int counter = 0;
            foreach (var i in db.Peoples.ToList())
            {
                people[counter] = new People() { Id = i.Id, Name = i.Name, Phone = i.Phone, FotoName = i.FotoName };
                counter++;
            }

            object jsonObject = new { people };
            return Json(jsonObject, JsonRequestBehavior.AllowGet); ;
        }

        // GET: Person/Details/{id}
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IPeopleRepository repo = new PeopleManager(db);
            People people = await repo.GetPeople(id);

            if (people == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string[] people_projacts = people.Projects.Select(x => x.Name).ToArray();

            object jsonObject = new { people.Name , people.Phone, people.FotoName, people_projacts };

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }

        //POST: Person/Create
        [HttpPost]
        public async Task<HttpResponseMessage> Create([Bind(Include = "Name, Phone")] People people, HttpPostedFileBase upload)
        {
            if (people.Name != null && people.Phone != null)
            {
                if (upload != null)
                {
                    string fileName = people.Id.ToString() + "_" + Path.GetFileName(upload.FileName);
                    string ext = Path.GetExtension(upload.FileName);

                    if (ext.ToLower() == ".png")
                    {
                        upload.SaveAs(Server.MapPath(Path.Combine("~/Fotos/", fileName)));
                        people.FotoName = fileName;
                    }
                }

                IPeopleRepository repo = new PeopleManager(db);
                int CreatePeopleId = await repo.CreatePeople(people);
                HttpResponseMessage responseMessage = new HttpResponseMessage() {StatusCode = HttpStatusCode.Created };
                responseMessage.Headers.Add("People_ID", CreatePeopleId.ToString());
                return responseMessage;
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        // POST: Person/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Phone")] People people, int id)
        {
            if (people.Name != null && people.Phone != null)
            {
                IPeopleRepository repo = new PeopleManager(db);
                bool e = await repo.EditPeople(people, id);

                if (e == false)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // GET: Person/Delete/{id}
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IPeopleRepository repo = new PeopleManager(db);
            bool e = await repo.DeletePeople(id);

            if (e == false)
            {
                return HttpNotFound();
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // POST: Person/EditAvatar/{id}
        public async Task<ActionResult> EditAvatar(HttpPostedFileBase new_avatar, int? id)
        {
            if (id != null)
            {
                IPeopleRepository repo = new PeopleManager(db);
                People people = await repo.GetPeople(id);

                if (new_avatar != null && people != null)
                {
                    string fileName = people.Id.ToString() + "_" + Path.GetFileName(new_avatar.FileName);
                    string ext = Path.GetExtension(new_avatar.FileName);

                    if (ext.ToLower() == ".png")
                    {
                        new_avatar.SaveAs(Server.MapPath(Path.Combine("~/Fotos/", fileName)));
                        people.FotoName = fileName;
                        await repo.EditPeople(people, id);
                        return new HttpStatusCodeResult(HttpStatusCode.OK);
                    }
                }
            }
                        
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Person/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Phone,FotoName")] People people)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(people).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(people);
        //}

            // POST: Person/Delete/5
            //[HttpPost, ActionName("Delete")]
            //[ValidateAntiForgeryToken]
            //public async Task<ActionResult> DeleteConfirmed(int id)
            //{
            //    People people = await db.Peoples.FindAsync(id);
            //    db.Peoples.Remove(people);
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}

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
