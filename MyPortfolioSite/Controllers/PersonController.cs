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

namespace MyPortfolioSite.Controllers
{
    public class PersonController : Controller
    {
        private DB_Contex db = new DB_Contex();

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
        public async Task<HttpResponseMessage> Create([Bind(Include = "Name, Phone")] People people)
        {
            if ((people.Name != null) && (people.Phone != null))
            {
                IPeopleRepository repo = new PeopleManager(db);
                int CreatePeopleId = await repo.CreatePeople(people);
                HttpResponseMessage responseMessage = new HttpResponseMessage() {StatusCode = HttpStatusCode.Created };
                responseMessage.Headers.Add("People_ID", CreatePeopleId.ToString());
                return responseMessage;
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
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

        // GET: Person/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    People people = await db.Peoples.FindAsync(id);
        //    if (people == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(people);
        //}

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
